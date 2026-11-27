// Builds the prompt sheet from prompts.json: sheet.html (the interactive page) and README.md (the same prompts as
// plain text), each with a ChatGPT and a Gemini version of every prompt.
// Run with `node art/reference-prompts/build.mjs` after editing prompts.json.
import { existsSync, mkdirSync, readFileSync, writeFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const data = JSON.parse(readFileSync(join(here, 'prompts.json'), 'utf8'));
const { styles, rules, chatgptAspect, gemini, pitch, headline, guides, sections, prompts } = data;
const byId = Object.fromEntries([...prompts, ...guides].map(p => [p.id, p]));
const isGuide = id => guides.some(g => g.id === id);
const guideDir = join(here, '..', 'reference', 'guides');
for (const g of guides) {
  if (!/^G\d\d$/.test(g.id) || !g.ref || !g.file || !g.about) throw new Error(`Guide ${g.id} needs an id like G01, a ref, a file and an about.`);
  if (!existsSync(join(guideDir, g.file + '.png'))) throw new Error(`Guide ${g.id}: render ${g.file}.png with art/blender/render_layout_guides.py first.`);
}

// ---------- Checks ----------
for (const [i, p] of prompts.entries()) {
  if (!styles[p.style] || !gemini.styles[p.style]) throw new Error(`${p.id}: unknown style ${p.style}`);
  if (!sections.some(s => s.id === p.section)) throw new Error(`${p.id}: unknown section ${p.section}`);
  if (!chatgptAspect[p.aspect] || !gemini.aspect[p.aspect]) throw new Error(`${p.id}: unknown aspect ${p.aspect}`);
  for (const a of p.attach) {
    if (!byId[a]) throw new Error(`${p.id}: attaches unknown ${a}`);
    if (!byId[a].ref) throw new Error(`${p.id}: attaches ${a}, which has no "ref" name for the Gemini prompt`);
    if (prompts.indexOf(byId[a]) > i) throw new Error(`${p.id}: attaches ${a}, which comes later`);
  }
}
if (prompts.some(p => p.attach.length > 4)) throw new Error('Keep attachments to four or fewer.');
for (const p of prompts) if (!/^[a-z0-9]+(-[a-z0-9]+)*$/.test(p.file || '')) throw new Error(`${p.id}: "file" must be kebab-case`);
if (new Set(prompts.map(p => p.file)).size !== prompts.length) throw new Error('File names must be unique.');
if (sections.some(s => !s.folder)) throw new Error('Every section needs a "folder".');
if (!pitch || !pitch.chatgpt || !pitch.gemini || !pitch.chatgptGuided || !pitch.geminiGuided) throw new Error('prompts.json needs plain and guided "pitch" blocks for both tools.');
if (!headline || !headline.chatgpt || !headline.gemini) throw new Error('prompts.json needs a "headline" for both tools.');
for (const p of prompts) if (p.attach.some(isGuide) && !p.pitch) throw new Error(`${p.id}: attaches a layout guide, so set "pitch": true`);
for (const p of prompts) if (typeof p.pitch !== 'boolean') throw new Error(`${p.id}: "pitch" must be true or false`);

// ---------- Where the images live in the repo ----------
const REF_ROOT = 'art/reference';
const REF_URL = 'https://github.com/tejamanne-alt/free-kick-classics/tree/HEAD/art/reference';
const sectionOf = p => sections.find(s => s.id === p.section);
const fileName = p => `${p.id}-${p.file}.png`;
const folderOf = p => `${REF_ROOT}/${sectionOf(p).folder}/`;
const filePath = p => folderOf(p) + fileName(p);

// ---------- Prompt composition ----------
const ORDINALS = ['first', 'second', 'third', 'fourth'];
const chatgptLead = p => `Make ${chatgptAspect[p.aspect]}.`;
function geminiLead(p) {
  const shape = `Create an image in ${gemini.aspect[p.aspect]}.`;
  if (!p.attach.length) return shape;
  const refs = p.attach.map(id => `${byId[id].ref} (${id})`);
  if (refs.length === 1) return `${shape} Use the attached image, ${refs[0]}, as your reference.`;
  return `${shape} Use the reference images attached in this order: ${refs.map((r, i) => `${ORDINALS[i]}, ${r}`).join('; ')}.`;
}
const geminiBody = p => p.geminiPrompt || p.prompt;
const hasGuide = p => p.attach.some(isGuide);
// Scenes open with a one-line style headline: image models weight the start of a prompt most.
const headlineFor = (p, tool) => (p.style === 'world' ? headline[tool] + ' ' : '');
function pitchBlock(p, tool) {
  if (!p.pitch) return '';
  const text = hasGuide(p) ? pitch[tool + 'Guided'] : pitch[tool];
  return text + '\n\n';
}

const TOOLS = {
  chatgpt: {
    name: 'ChatGPT',
    visible: p => [`${chatgptLead(p)} ${headlineFor(p, 'chatgpt')}${p.prompt}`],
    full: p => `${chatgptLead(p)} ${headlineFor(p, 'chatgpt')}${p.prompt}\n\n${pitchBlock(p, 'chatgpt')}${styles[p.style].text}\n\n${rules}`,
    styleName: k => styles[k].name,
    styleText: k => styles[k].text,
    rules,
  },
  gemini: {
    name: 'Gemini',
    visible: p => [geminiLead(p), headlineFor(p, 'gemini') + geminiBody(p)],
    full: p => `${geminiLead(p)}\n\n${headlineFor(p, 'gemini')}${geminiBody(p)}\n\n${pitchBlock(p, 'gemini')}${gemini.styles[p.style]}\n\n${gemini.rules}`,
    styleName: k => styles[k].name,
    styleText: k => gemini.styles[k],
    rules: gemini.rules,
  },
};
const toolKeys = Object.keys(TOOLS);
const firstCount = prompts.filter(p => p.batch === 'first').length;

const notes = {
  chatgpt: [
    'Each prompt opens with the image shape, and ChatGPT picks landscape, portrait or square from it.',
    'Attach the listed images, then paste the prompt into the same message.',
    'To fix something, reply in the same chat, for example: “Same sheet, but flat orthographic views with no shadows.”',
  ],
  gemini: [
    'Upload the listed images first, in the order the card shows. The prompt names them in that order.',
    'Gemini follows descriptions of what to include better than lists of what to avoid, so these prompts describe clean, plain surfaces instead of banning logos one by one.',
    'Refine in the same chat, for example: “Keep everything the same, but make the background plain light grey.”',
    'If an image comes back in the wrong shape, reply: “Recompose this as a 3:2 landscape image.”',
  ],
};

const scenes = [
  ['The comic render setup', 'One look for every scene, over realistic geometry and textures: Line Art ink outlines and hatching, halftone and Ben-Day shading, off-register colour instead of blur, a paper-print texture, and a print style per era.'],
  ['The harbour stadium', 'The first playable ground, at golden hour and under floodlights, with the pitch and its markings built to regulation dimensions: stands, crowd, dugouts, boards, sky, sea and lighting, plus the phone cameras for kicker, keeper and the split-panel stare-down.'],
  ['The free-kick set piece', 'The same ground with a wall at the 9.15 m spray line and the referee.'],
  ['Two more eras', 'The 1970s old ground on a winter afternoon and the 1990s floodlit rain, each lit and graded as its own ground.'],
  ['Classics scenes', 'Clifftop coast, night storm, snow, monsoon and mountain altitude, one per scenario card.'],
];
const animations = [
  ['Kicker', 'Ready idle, run-up, stutter, side-foot, driven knuckleball, curler, trivela, chip, two celebrations, a miss. Animated on twos, with smear frames on the strikes.'],
  ['Keeper', 'Ready stance, shuffle, low, middle and top-corner dives to both sides, staying big, catch, smother, fingertip save, punch, celebration, despair.'],
  ['Wall', 'Line up, jump, brace, the lying man.'],
  ['Ball and effects', 'Inked replay trajectories with speed lines that follow flight paths exported from the game\'s physics, impact bursts on the net, scarves and flags, rain, snow and flare smoke.'],
];

const esc = s => String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');

// ---------- README.md ----------
const md = [];
md.push('# Reference art prompts', '');
md.push('Image prompts to generate in ChatGPT or Gemini. The images come back to me as references for building the Blender scenes and animations.', '');
md.push(`There are ${prompts.length} prompts, each in a ChatGPT and a Gemini version. Make the ${firstCount} in the first batch before the others.`, '');
md.push('Edit `prompts.json`, then run `node art/reference-prompts/build.mjs` to rebuild this file and `sheet.html`.', '');
md.push('## How to use them', '');
md.push('1. Generate **A01** first, in a new chat. Make three or four variations and keep the one you like best.');
md.push('2. For every prompt, attach exactly the images listed under **Attach**, in that order. IDs starting with **G** are the layout guides below.');
md.push('3. Give each character its own chat, so the face and kit stay the same across that character\'s sheets.');
md.push('4. Save each image under the name given below, e.g. `C01-kicker-turnaround.png`. Alternatives get `-v2`, `-v3`.', '');
md.push('## Layout guides', '');
md.push('Image models can\'t measure, so prompts that show the pitch come with a layout guide: regulation markings, the goal and the players\' positions, rendered in Blender from the exact camera. The model copies the layout and takes the look from the prompt. Attach the guide where a prompt lists it.', '');
md.push('| Guide | File | Shape | Used by |', '| --- | --- | --- | --- |');
for (const g of guides) md.push(`| ${g.id} · ${g.title} | \`${REF_ROOT}/guides/${g.file}.png\` | ${g.aspect} | ${prompts.filter(p => p.attach.includes(g.id)).map(p => p.id).join(', ')} |`);
md.push('', 'Re-render them with `xvfb-run -a blender -b --python art/blender/render_layout_guides.py -- art/reference/guides`.', '');
md.push('## Where the files go', '');
md.push(`Every image has a fixed place under \`${REF_ROOT}/\`, one folder per section:`, '');
md.push('| Folder | What goes there |', '| --- | --- |');
for (const s of sections) md.push(`| \`${REF_ROOT}/${s.folder}/\` | ${s.name} (${prompts.filter(p => p.section === s.id).map(p => p.id).join(', ')}) |`);
md.push('', 'Two ways to get them there: attach them in our chat and I\'ll file them, or open the folder on GitHub and use **Add file → Upload files**. If a download comes out as `.jpg` or `.webp`, keep that extension; the name before it is what matters.', '');
for (const key of toolKeys) {
  md.push(`**${TOOLS[key].name}**`, '');
  for (const n of notes[key]) md.push(`- ${n}`);
  md.push('');
}
md.push('Each prompt below is complete: its style block, the pitch-markings block when the pitch is in shot, and the originality rules. Copy the whole block for the tool you use.', '');
for (const section of sections) {
  md.push(`## ${section.name}`, '', section.blurb, '');
  for (const p of prompts.filter(q => q.section === section.id)) {
    md.push(`### ${p.id} · ${p.title}`, '');
    md.push(`- **Batch:** ${p.batch === 'first' ? 'first' : 'later'}`);
    md.push(`- **Image shape:** ${p.aspect}`);
    md.push(`- **Save as:** \`${filePath(p)}\``);
    md.push(`- **Attach, in order:** ${p.attach.length ? p.attach.join(', ') : 'nothing'}`);
    md.push(`- **I'll build:** ${p.builds}`);
    if (p.tip) md.push(`- **Tip:** ${p.tip}`);
    for (const key of toolKeys) md.push('', `**${TOOLS[key].name}**`, '', '```text', TOOLS[key].full(p), '```');
    md.push('');
  }
}
md.push('## What I\'ll build in Blender', '');
md.push('**Scenes**', '');
for (const [name, text] of scenes) md.push(`- **${name}.** ${text}`);
md.push('', '**Animations**', '');
for (const [name, text] of animations) md.push(`- **${name}.** ${text}`);
md.push('', 'You get `.blend` scenes, FBX characters and animations ready for Unity, and preview renders and videos along the way.', '');
md.push('## Sending things back', '');
md.push(`- **Images:** save each under the name on its prompt (alternatives get \`-v2\`), then attach them in our chat or upload them to their folder under \`${REF_ROOT}/\`.`);
md.push('- **Models:** any format Blender opens: `.blend`, `.fbx`, `.glb`/`.gltf` or `.obj`. Metres as units is ideal. A humanoid rig is a bonus; I can rig unrigged characters.');
md.push('- **Rights:** only send models and images you made or are allowed to use in a commercial game, and tell me any credit a licence requires.');
writeFileSync(join(here, 'README.md'), md.join('\n') + '\n');

// ---------- art/reference folder READMEs ----------
const refDir = join(here, '..', 'reference');
const overview = ['# Reference images', ''];
overview.push('Images generated from the prompts in [`art/reference-prompts`](../reference-prompts/README.md), used as references for the Blender scenes and animations. Each prompt has a fixed file name and folder; the prompt sheet shows both, with copy buttons.', '');
overview.push('| Folder | Images |', '| --- | --- |');
for (const s of sections) overview.push(`| [\`${s.folder}/\`](${s.folder}/README.md) | ${s.name}: ${prompts.filter(p => p.section === s.id).length} |`);
overview.push(`| [\`guides/\`](guides/README.md) | Layout guides rendered in Blender, attached to the prompts: ${guides.length} |`);
overview.push('', '- Name each file exactly as listed, e.g. `C01-kicker-turnaround.png`. Alternatives get `-v2`, `-v3`.');
overview.push('- To add images on GitHub, open the folder and use **Add file → Upload files**. Or attach them in the chat with Claude, who files them.');
overview.push('- Keep whatever extension the download has (`.png`, `.jpg` or `.webp`).');
overview.push('', 'This folder is generated by `node art/reference-prompts/build.mjs`; edit the prompts there.');
mkdirSync(refDir, { recursive: true });
writeFileSync(join(refDir, 'README.md'), overview.join('\n') + '\n');
{
  const lines = ['# Layout guides', '', 'Rendered in Blender by `art/blender/render_layout_guides.py`: regulation markings, the goal and the players\' positions from each game camera, in flat placeholder colours. Attach them to the prompts that list them so image models copy the layout exactly.', ''];
  lines.push('| File | Guide | Shape | Used by |', '| --- | --- | --- | --- |');
  for (const g of guides) lines.push(`| \`${g.file}.png\` | ${g.id} · ${g.title}: ${g.about} | ${g.aspect} | ${prompts.filter(p => p.attach.includes(g.id)).map(p => p.id).join(', ')} |`);
  mkdirSync(guideDir, { recursive: true });
  writeFileSync(join(guideDir, 'README.md'), lines.join('\n') + '\n');
}
for (const s of sections) {
  const lines = [`# ${s.name}`, '', s.blurb, '', 'Save each image here under the name below. Alternatives get `-v2`, `-v3`.', ''];
  lines.push('| File | Prompt | Batch |', '| --- | --- | --- |');
  for (const p of prompts.filter(q => q.section === s.id)) lines.push(`| \`${fileName(p)}\` | ${p.id} · ${p.title} | ${p.batch} |`);
  lines.push('', `The prompts are in [\`art/reference-prompts\`](../../reference-prompts/README.md#${s.name.toLowerCase().replace(/[^a-z0-9 ]/g, '').replace(/ /g, '-')}).`);
  mkdirSync(join(refDir, s.folder), { recursive: true });
  writeFileSync(join(refDir, s.folder, 'README.md'), lines.join('\n') + '\n');
}

// ---------- sheet.html ----------
const toolSwitch = id => `
      <div class="tool-switch" role="radiogroup" aria-label="Prompts for">
        ${toolKeys.map(k => `<label><input type="radio" name="${id}" value="${k}" data-tool-choice${k === 'chatgpt' ? ' checked' : ''}><span>${TOOLS[k].name}</span></label>`).join('')}
      </div>`;
const sectionNav = sections.map(s => `<button class="chip" type="button" data-filter="${s.id}">${esc(s.name)}</button>`).join('');
const attachList = p => p.attach.length
  ? p.attach.map(a => `<a class="ref${isGuide(a) ? ' guide-ref' : ''}" href="#${isGuide(a) ? 'guide-' + a : a}">${a}</a>`).join('<span class="then">then</span>')
  : '<span class="none">Nothing</span>';
const cards = sections.map(section => {
  const items = prompts.filter(p => p.section === section.id).map(p => `
      <article class="card" id="${p.id}" data-section="${p.section}" data-batch="${p.batch}">
        <header class="card-head">
          <span class="pid">${p.id}</span>
          <h3>${esc(p.title)}</h3>
        </header>
        <dl class="meta">
          <div><dt>Batch</dt><dd><span class="badge ${p.batch}">${p.batch === 'first' ? 'First batch' : 'Later'}</span></dd></div>
          <div><dt>Shape</dt><dd>${esc(p.aspect)}</dd></div>
          <div><dt>Attach</dt><dd>${attachList(p)}</dd></div>
        </dl>
        <div class="file">
          <span class="file-label">Save as</span>
          <code class="path"><span class="dir">${esc(folderOf(p))}</span><span class="name">${esc(fileName(p))}</span></code>
          <span class="file-actions">
            <button class="copy small" type="button" data-copy-file="name" data-id="${p.id}">Copy name</button>
            <button class="copy small" type="button" data-copy-file="path" data-id="${p.id}">Copy path</button>
          </span>
        </div>
        <p class="builds"><span>I'll build</span>${esc(p.builds)}</p>
        ${toolKeys.map(k => `<div class="prompt only-${k}">${TOOLS[k].visible(p).map(t => `<p>${esc(t)}</p>`).join('')}</div>`).join('\n        ')}
        <p class="appended">Copies with the ${esc(styles[p.style].name.toLowerCase())}${p.pitch ? (hasGuide(p) ? ', the pitch markings (copied from the layout guide)' : ', the pitch markings') : ''} and the originality rules for <span class="only-chatgpt">ChatGPT</span><span class="only-gemini">Gemini</span>.</p>
        ${p.tip ? `<p class="tip"><span>Tip</span>${esc(p.tip)}</p>` : ''}
        <div class="actions">
          <button class="copy" type="button" data-copy="${p.id}">Copy <span class="only-chatgpt">ChatGPT</span><span class="only-gemini">Gemini</span> prompt</button>
          <label class="done"><input type="checkbox" id="done-${p.id}" data-done="${p.id}"> Made it</label>
          <span class="status" role="status" aria-live="polite"></span>
        </div>
        <textarea class="fallback" id="fallback-${p.id}" readonly hidden aria-label="Full prompt for ${p.id}"></textarea>
      </article>`).join('');
  return `
    <section class="group" data-group="${section.id}">
      <div class="group-head">
        <h2>${esc(section.name)}</h2>
        <p>${esc(section.blurb)}</p>
      </div>${items}
    </section>`;
}).join('');

const styleBlocks = toolKeys.map(k => `
      <div class="blocks only-${k}">${Object.keys(styles).map(s => `
        <div class="block">
          <div class="block-head"><h3>${esc(TOOLS[k].styleName(s))}</h3><span>${esc(styles[s].use)}</span>
            <button class="copy small" type="button" data-copy-style="${k}:${s}">Copy</button></div>
          <p>${esc(TOOLS[k].styleText(s))}</p>
        </div>`).join('')}
        <div class="block">
          <div class="block-head"><h3>${esc(pitch.name)}</h3><span>${esc(pitch.use)}</span>
            <button class="copy small" type="button" data-copy-style="${k}:pitch">Copy</button></div>
          <p>${esc(pitch[k])}</p>
        </div>
        <div class="block">
          <div class="block-head"><h3>${esc(pitch.guidedName)}</h3><span>Prompts with a G guide attached</span>
            <button class="copy small" type="button" data-copy-style="${k}:pitchGuided">Copy</button></div>
          <p>${esc(pitch[k + 'Guided'])}</p>
        </div>
        <div class="block">
          <div class="block-head"><h3>Originality rules</h3><span>Every prompt</span>
            <button class="copy small" type="button" data-copy-style="${k}:rules">Copy</button></div>
          <p>${esc(TOOLS[k].rules)}</p>
        </div>
      </div>`).join('');

const payload = { files: Object.fromEntries(prompts.map(p => [p.id, { name: fileName(p), path: filePath(p) }])) };
for (const k of toolKeys) {
  payload[k] = { prompts: Object.fromEntries(prompts.map(p => [p.id, TOOLS[k].full(p)])), styles: {} };
  for (const s of Object.keys(styles)) payload[k].styles[s] = TOOLS[k].styleText(s);
  payload[k].styles.rules = TOOLS[k].rules;
  payload[k].styles.pitch = pitch[k];
  payload[k].styles.pitchGuided = pitch[k + 'Guided'];
}

const html = `<title>Free Kick Classics Prompts</title>
<meta name="claude:viewport-fit-sides" content="cover">
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Bricolage+Grotesque:opsz,wght@12..96,500;12..96,700&family=Inter:wght@400;500;600&family=JetBrains+Mono:wght@500&display=swap">
<style>
  /* Layout: one reading column, like a match-day comic annual read under the floodlights. Deliberately dark-only: evening-match palette with halftone accents. */
  :root {
    color-scheme: dark;
    --night: #0f0e1d;
    --deep: #181630;
    --raised: #211e3d;
    --rule: #302c52;
    --ink: #f2ede3;
    --ink-2: #bdb6cf;
    --muted: #8f88a8;
    --gold: #eab95f;
    --teal: #4cc6b5;
    --display: "Bricolage Grotesque", "Avenir Next", "Segoe UI", system-ui, sans-serif;
    --body: "Inter", system-ui, -apple-system, "Segoe UI", sans-serif;
    --mono: "JetBrains Mono", ui-monospace, "SF Mono", Menlo, monospace;
  }
  * { box-sizing: border-box; }
  body { background: var(--night); color: var(--ink); font-family: var(--body); font-size: 15px; line-height: 1.6; }
  body:not([data-tool="gemini"]) .only-gemini, body[data-tool="gemini"] .only-chatgpt { display: none !important; }
  a { color: var(--gold); }
  code { font-family: var(--mono); font-size: 13px; color: var(--gold); }
  .wrap { max-width: 880px; margin: 0 auto; padding-left: max(16px, env(safe-area-inset-left, 0px)); padding-right: max(16px, env(safe-area-inset-right, 0px)); }

  .hero { margin-top: calc(-1 * env(safe-area-inset-top, 0px)); padding-top: calc(env(safe-area-inset-top, 0px) + 3.5rem); padding-bottom: 2.75rem;
    background:
      radial-gradient(55% 85% at 15% 100%, rgba(234, 165, 80, 0.30), transparent 70%),
      radial-gradient(60% 80% at 80% 0%, rgba(70, 120, 170, 0.32), transparent 70%),
      radial-gradient(30% 50% at 92% 70%, rgba(255, 240, 210, 0.10), transparent 70%),
      linear-gradient(180deg, #14182b 0%, var(--night) 100%);
    border-bottom: 1px solid var(--rule); position: relative; isolation: isolate; }
  .hero::after { content: ""; position: absolute; inset: 0; z-index: -1; pointer-events: none;
    background-image: radial-gradient(circle, rgba(234, 185, 95, 0.22) 1.2px, transparent 1.8px); background-size: 8px 8px;
    -webkit-mask-image: radial-gradient(60% 90% at 100% 100%, #000, transparent 70%); mask-image: radial-gradient(60% 90% at 100% 100%, #000, transparent 70%); }
  .eyebrow { font-family: var(--mono); font-size: 12px; letter-spacing: 0.08em; text-transform: uppercase; color: var(--gold); margin: 0 0 0.75rem; }
  h1 { font-family: var(--display); font-weight: 700; font-size: clamp(2.1rem, 5.5vw, 3.4rem); line-height: 1.04; letter-spacing: -0.02em; margin: 0 0 1rem; text-wrap: balance; max-width: 16ch; }
  .lead { font-size: 1.08rem; color: var(--ink-2); max-width: 62ch; margin: 0 0 1.5rem; }
  .hero-row { display: flex; flex-wrap: wrap; align-items: center; gap: 1rem 1.75rem; }
  .facts { display: flex; flex-wrap: wrap; gap: 0.5rem 1.5rem; font-family: var(--mono); font-size: 13px; color: var(--ink-2); margin: 0; padding: 0; list-style: none; }
  .facts b { color: var(--ink); font-weight: 500; }

  .tool-switch { display: inline-flex; flex: none; padding: 3px; border: 1px solid var(--rule); border-radius: 999px; background: rgba(15, 14, 29, 0.6); }
  .tool-switch label { position: relative; cursor: pointer; }
  .tool-switch input { position: absolute; opacity: 0; inset: 0; margin: 0; cursor: pointer; }
  .tool-switch span { display: block; font: 600 13px/1 var(--body); color: var(--ink-2); padding: 0.5rem 0.9rem; border-radius: 999px; }
  .tool-switch input:checked + span { background: var(--gold); color: var(--night); }
  .tool-switch input:focus-visible + span { outline: 2px solid var(--gold); outline-offset: 2px; }

  main section.intro { padding-block: 2.5rem 0.5rem; }
  h2 { font-family: var(--display); font-weight: 700; font-size: 1.6rem; letter-spacing: -0.01em; margin: 0 0 0.5rem; text-wrap: balance; }
  .intro > p { color: var(--ink-2); max-width: 65ch; margin: 0 0 1.25rem; }
  .pillars { display: grid; gap: 0.9rem; margin: 0 0 1.75rem; padding: 0; list-style: none; }
  .pillars li { display: grid; grid-template-columns: 9.5rem 1fr; gap: 1rem; padding-bottom: 0.9rem; border-bottom: 1px solid var(--rule); }
  .pillars strong { font-family: var(--display); font-weight: 500; font-size: 1.05rem; color: var(--ink); }
  .pillars span { color: var(--ink-2); }
  .eras { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 0.75rem; }
  .era { position: relative; overflow: hidden; isolation: isolate; border-radius: 10px; padding: 0.9rem 1rem 1rem; min-height: 8.5rem; display: flex; flex-direction: column; justify-content: flex-end; border: 2px solid #0b0a12; box-shadow: 4px 4px 0 #0b0a12; }
  .era::after { content: ""; position: absolute; inset: 0; z-index: -1; pointer-events: none;
    background-image: radial-gradient(circle, rgba(11, 10, 18, 0.55) 1.1px, transparent 1.6px); background-size: 6px 6px;
    -webkit-mask-image: linear-gradient(20deg, transparent 40%, #000 95%); mask-image: linear-gradient(20deg, transparent 40%, #000 95%); }
  .era.seventies::after { background-size: 9px 9px; background-image: radial-gradient(circle, rgba(90, 50, 20, 0.5) 1.8px, transparent 2.4px); }
  .era.seventies { filter: saturate(0.85); }
  .era h3 { font-family: var(--display); font-weight: 700; font-size: 1.05rem; margin: 0; color: #fff; text-shadow: 0 1px 8px rgba(0,0,0,0.45); }
  .era p { margin: 0.2rem 0 0; font-size: 13px; line-height: 1.4; color: rgba(255,255,255,0.88); text-shadow: 0 1px 6px rgba(0,0,0,0.5); }
  .era.seventies { background: linear-gradient(165deg, #d9b98a 0%, #a8774a 45%, #4a3a2c 100%); }
  .era.nineties { background: radial-gradient(60% 55% at 75% 12%, rgba(236, 244, 255, 0.85) 0%, transparent 60%), linear-gradient(180deg, #2a3a57 0%, #15213a 55%, #0d2a1e 100%); }
  .era.modern { background: radial-gradient(70% 60% at 25% 78%, #f6c36b 0%, transparent 60%), linear-gradient(180deg, #2f5d86 0%, #c9784a 72%, #3a2a2a 100%); }
  .era .tag { font-family: var(--mono); font-size: 11px; letter-spacing: 0.06em; text-transform: uppercase; color: rgba(255,255,255,0.85); margin-bottom: auto; }

  ol.steps { counter-reset: step; list-style: none; margin: 0 0 1.25rem; padding: 0; display: grid; gap: 0.75rem; }
  ol.steps li { counter-increment: step; display: grid; grid-template-columns: 2rem 1fr; gap: 0.75rem; align-items: start; color: var(--ink-2); }
  ol.steps li::before { content: counter(step); font-family: var(--mono); font-size: 13px; color: var(--night); background: var(--gold); width: 1.6rem; height: 1.6rem; border-radius: 50%; display: grid; place-items: center; margin-top: 0.1rem; }
  ol.steps b { color: var(--ink); font-weight: 600; }
  .notes { border-left: 2px solid var(--gold); padding: 0.1rem 0 0.1rem 1rem; margin: 0 0 1.25rem; }
  .notes h3 { font-size: 0.95rem; font-weight: 600; margin: 0 0 0.4rem; }
  .notes ul { margin: 0; padding-left: 1.1rem; color: var(--ink-2); display: grid; gap: 0.35rem; }

  details.styles { border: 1px solid var(--rule); border-radius: 10px; background: var(--deep); margin: 0 0 1rem; }
  details.styles summary { cursor: pointer; padding: 0.9rem 1rem; font-weight: 600; color: var(--ink); }
  details.styles .blocks { display: grid; gap: 1rem; padding: 0 1rem 1rem; }
  .block-head { display: flex; flex-wrap: wrap; align-items: baseline; gap: 0.4rem 0.75rem; }
  .block h3 { font-size: 0.95rem; margin: 0; font-weight: 600; }
  .block-head span { font-size: 13px; color: var(--muted); }
  .block-head .copy { margin-left: auto; }
  .block p { margin: 0.4rem 0 0; color: var(--ink-2); font-size: 14px; }

  .bar { position: sticky; top: env(safe-area-inset-top, 0px); z-index: 5; background: rgba(15, 14, 29, 0.94); backdrop-filter: blur(8px); border-bottom: 1px solid var(--rule); }
  body::before { content: ""; position: fixed; inset: 0 0 auto; height: env(safe-area-inset-top, 0px); z-index: 10; background: var(--night); }
  .bar-inner { display: flex; align-items: center; gap: 0.6rem 0.75rem; padding-block: 0.6rem; }
  .chips { display: flex; gap: 0.4rem; overflow-x: auto; scrollbar-width: none; flex: 1; min-width: 0; padding-right: 1.5rem;
    -webkit-mask-image: linear-gradient(90deg, #000 calc(100% - 2rem), transparent); mask-image: linear-gradient(90deg, #000 calc(100% - 2rem), transparent); }
  .chips::-webkit-scrollbar { display: none; }
  .chip { flex: none; font: 500 13px/1 var(--body); color: var(--ink-2); background: transparent; border: 1px solid var(--rule); border-radius: 999px; padding: 0.5rem 0.8rem; cursor: pointer; }
  .chip:hover { border-color: var(--muted); color: var(--ink); }
  .chip[aria-pressed="true"] { background: var(--ink); color: var(--night); border-color: var(--ink); }
  .progress { flex: none; font-family: var(--mono); font-size: 12px; color: var(--ink-2); white-space: nowrap; font-variant-numeric: tabular-nums; }
  .progress b { color: var(--teal); font-weight: 500; }

  .group { padding-top: 2.25rem; }
  .group-head p { color: var(--ink-2); margin: 0 0 1rem; max-width: 65ch; }
  .card { background: var(--deep); border: 1px solid var(--rule); border-radius: 12px; padding: 1.1rem 1.2rem 1rem; margin-bottom: 0.9rem; scroll-margin-top: 7rem; }
  .card.is-done { border-color: rgba(76, 198, 181, 0.45); }
  .card-head { display: flex; align-items: baseline; gap: 0.75rem; }
  .pid { font-family: var(--mono); font-size: 13px; color: var(--gold); flex: none; }
  .card h3 { font-family: var(--display); font-weight: 500; font-size: 1.2rem; line-height: 1.25; margin: 0; text-wrap: balance; }
  .meta { display: flex; flex-wrap: wrap; gap: 0.3rem 1.4rem; margin: 0.55rem 0 0.6rem; font-size: 13px; }
  .meta div { display: flex; gap: 0.45rem; align-items: baseline; }
  .meta dt { color: var(--muted); }
  .meta dd { margin: 0; color: var(--ink-2); }
  .badge { font-size: 12px; padding: 0.1rem 0.5rem; border-radius: 999px; border: 1px solid; }
  .badge.first { color: var(--teal); border-color: rgba(76, 198, 181, 0.5); }
  .badge.later { color: var(--muted); border-color: var(--rule); }
  .ref { font-family: var(--mono); font-size: 12px; text-decoration: none; border-bottom: 1px solid rgba(234, 185, 95, 0.4); }
  .ref:hover, .ref:focus-visible { border-bottom-color: var(--gold); }
  .then { color: var(--muted); margin: 0 0.35rem; font-size: 12px; }
  .none { color: var(--muted); }
  .file { display: flex; flex-wrap: wrap; align-items: center; gap: 0.4rem 0.6rem; margin: 0 0 0.65rem; padding: 0.5rem 0.7rem; border: 1px dashed var(--rule); border-radius: 8px; }
  .file-label { font-family: var(--mono); font-size: 11px; letter-spacing: 0.06em; text-transform: uppercase; color: var(--muted); }
  .path { font-family: var(--mono); font-size: 13px; overflow-wrap: anywhere; min-width: 0; flex: 1 1 16rem; }
  .path .dir { color: var(--muted); }
  .path .name { color: var(--ink); }
  .file-actions { display: inline-flex; gap: 0.4rem; flex: none; }
  .guides { margin: 0 0 1.75rem; }
  .guides h3 { font-size: 0.95rem; font-weight: 600; margin: 0 0 0.4rem; }
  .guides > p { color: var(--ink-2); margin: 0 0 0.9rem; max-width: 70ch; }
  .guide-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(min(100%, 15rem), 1fr)); gap: 0.9rem; }
  .guide { margin: 0; background: var(--deep); border: 1px solid var(--rule); border-radius: 10px; overflow: hidden; scroll-margin-top: 7rem; }
  .guide img { display: block; width: 100%; height: auto; background: #d6d8dc; }
  .guide.portrait img { max-height: 22rem; object-fit: contain; }
  .guide figcaption { display: grid; gap: 0.2rem; padding: 0.65rem 0.8rem 0.8rem; font-size: 13px; color: var(--ink-2); }
  .guide figcaption b { color: var(--ink); font-weight: 600; }
  .guide .about { line-height: 1.45; }
  .guide .used { color: var(--muted); font-size: 12px; }
  .guide code { font-size: 12px; overflow-wrap: anywhere; }
  .ref.guide-ref { color: var(--teal); border-bottom-color: rgba(76, 198, 181, 0.45); }
  .where { margin: 0 0 1.25rem; }
  .where h3 { font-size: 0.95rem; font-weight: 600; margin: 0 0 0.4rem; }
  .where p { color: var(--ink-2); margin: 0 0 0.75rem; max-width: 70ch; }
  .where b { color: var(--ink); font-weight: 600; }
  .folders { overflow-x: auto; margin: 0 0 0.75rem; }
  .folders table { border-collapse: collapse; font-size: 14px; min-width: 100%; }
  .folders th { text-align: left; font-weight: 600; font-size: 12px; color: var(--muted); padding: 0 1rem 0.4rem 0; border-bottom: 1px solid var(--rule); }
  .folders td { padding: 0.4rem 1rem 0.4rem 0; border-bottom: 1px solid var(--rule); color: var(--ink-2); vertical-align: top; }
  .folders td:first-child { white-space: nowrap; }
  .folders .ids { font-family: var(--mono); font-size: 11.5px; color: var(--muted); margin-left: 0.35rem; }
  .builds, .tip { margin: 0 0 0.6rem; font-size: 14px; color: var(--ink-2); }
  .builds span, .tip span { font-family: var(--mono); font-size: 11px; letter-spacing: 0.06em; text-transform: uppercase; color: var(--muted); margin-right: 0.6rem; }
  .prompt { margin: 0 0 0.4rem; padding: 0.85rem 1rem; background: var(--raised); border-radius: 8px; color: var(--ink); font-size: 15px; display: grid; gap: 0.6rem; }
  .prompt p { margin: 0; }
  .appended { margin: 0 0 0.6rem; font-size: 12.5px; color: var(--muted); }
  .actions { display: flex; flex-wrap: wrap; align-items: center; gap: 0.6rem 1rem; }
  .copy { font: 600 14px/1 var(--body); color: var(--night); background: var(--gold); border: 0; border-radius: 8px; padding: 0.65rem 1rem; cursor: pointer; }
  .copy:hover { background: #f3c977; }
  .copy.small { font-size: 12.5px; padding: 0.4rem 0.7rem; background: transparent; color: var(--gold); border: 1px solid rgba(234, 185, 95, 0.5); }
  .done { display: inline-flex; align-items: center; gap: 0.45rem; font-size: 14px; color: var(--ink-2); cursor: pointer; }
  .done input { width: 1.05rem; height: 1.05rem; accent-color: var(--teal); }
  .status { font-size: 13px; color: var(--teal); }
  .fallback { width: 100%; min-height: 9rem; margin-top: 0.75rem; background: var(--night); color: var(--ink); border: 1px solid var(--gold); border-radius: 8px; padding: 0.75rem; font: 13px/1.5 var(--mono); }
  button:focus-visible, .chip:focus-visible, a:focus-visible, input:focus-visible, summary:focus-visible { outline: 2px solid var(--gold); outline-offset: 2px; }

  .outro { padding-block: 2.5rem 3rem; border-top: 1px solid var(--rule); margin-top: 2rem; }
  .outro h2 { margin-top: 0; }
  .outro h3 { font-family: var(--display); font-weight: 500; font-size: 1.1rem; margin: 1.25rem 0 0.4rem; }
  .outro ul { margin: 0; padding-left: 1.1rem; color: var(--ink-2); display: grid; gap: 0.4rem; }
  .outro li b { color: var(--ink); font-weight: 600; }

  @media (max-width: 640px) {
    .pillars li { grid-template-columns: 1fr; gap: 0.15rem; }
    .eras { grid-template-columns: 1fr; }
    .era { min-height: 6.5rem; }
    .card { padding: 1rem; }
    .bar-inner { flex-wrap: wrap; }
    .bar-inner .progress { margin-left: auto; }
    .chips { order: 3; flex-basis: 100%; }
  }
  @media (prefers-reduced-motion: no-preference) {
    .card { transition: border-color 0.2s ease; }
  }
</style>

<header class="hero">
  <div class="wrap">
    <p class="eyebrow">Free Kick Classics · reference art</p>
    <h1>Image prompts for the Blender build</h1>
    <p class="lead">Generate these in ChatGPT or Gemini and send the images back. I'll model, rig, light and animate the game's scenes from them in Blender, with every ball flight taken from the game's own physics.</p>
    <div class="hero-row">${toolSwitch('tool-hero')}
      <ul class="facts">
        <li><b>${prompts.length}</b> prompts</li>
        <li><b>${firstCount}</b> in the first batch</li>
        <li>Start with <b>A01</b></li>
      </ul>
    </div>
  </div>
</header>

<main>
  <section class="intro wrap" aria-labelledby="look">
    <h2 id="look">The look</h2>
    <p>Believable football grounds drawn as a comic book come to life. The architecture, turf, crowds and markings are true to life, down to regulation lines copied from layout guides; every surface is drawn with ink outlines, halftone dots and off-register colour. The goal, the ball and the two players stay boldest, because the game is still about reading your friend.</p>
    <ul class="pillars">
      <li><strong>A comic come to life</strong><span>The whole frame is drawn: ink outlines and hatching, halftone and Ben-Day dots, off-register colour instead of blur, graphic flares, speed lines and smears. Characters move on twos.</span></li>
      <li><strong>Lines you could referee by</strong><span>Markings copied from layout guides rendered at regulation size from each camera, painted like real lines on natural grass, with worn patches round the spot.</span></li>
      <li><strong>Places worth the trip</strong><span>A harbour at sunset, an old terraced ground, a clifftop, snow, a monsoon final, the mountains, each with real match-day detail: boards, dugouts, stewards, photographers.</span></li>
      <li><strong>Every era, its own print</strong><span>The 1970s drawn as yellowed newsprint, the 1990s as a glossy, heavily inked comic, the modern game as a crisp graphic novel.</span></li>
      <li><strong>Physics you can read</strong><span>Replays draw the ball's real flight as a bold inked trajectory with speed lines: the curl, dip or knuckle wobble from the flight model.</span></li>
      <li><strong>Faces carry the duel</strong><span>The stare-down plays out in split panels: the bluff, the read, the relief. Ball, frame and players always hold the strongest contrast.</span></li>
    </ul>
    <div class="eras" role="list">
      <div class="era seventies" role="listitem"><span class="tag">Heavy leather ball</span><h3>1970s</h3><p>Yellowed newsprint: coarse dots, off-register colour, the old ground in winter sun.</p></div>
      <div class="era nineties" role="listitem"><span class="tag">Big-bending glossy ball</span><h3>1990s</h3><p>Glossy 90s comic: heavy inks, floodlit rain, steam off the crowd.</p></div>
      <div class="era modern" role="listitem"><span class="tag">Light knuckling ball</span><h3>Modern</h3><p>Crisp graphic novel: the harbour stadium at golden hour. First to build.</p></div>
    </div>
  </section>

  <section class="intro wrap" aria-labelledby="how">
    <h2 id="how">How to use this sheet</h2>
    <ol class="steps">
      <li><span><b>Save the layout guides below</b> (G01–G05). They fix the pitch markings and camera for every prompt that shows the pitch.</span></li>
      <li><span><b>Make A01 next</b> in a new chat, with G01 attached. Ask for three or four variations and keep the one you love; it sets the look for everything else.</span></li>
      <li><span><b>Attach exactly what each card lists</b>, in that order: A01 first, then any character sheets, then the layout guide.</span></li>
      <li><span><b>One chat per character.</b> The kicker, keeper and wall each get their own chat, so faces and kits stay the same from sheet to sheet.</span></li>
      <li><span><b>Save each image under the name on its card</b>, like <code>C01-kicker-turnaround.png</code>, and tick “Made it”. Alternatives get <code>-v2</code>, <code>-v3</code>.</span></li>
    </ol>
    <div class="guides" id="guides">
      <h3>Layout guides</h3>
      <p>Image models can't measure, so a list of distances doesn't get the markings right. These guides are rendered in Blender at regulation dimensions from each game camera: the goal, every line, and grey figures where the players stand. The model copies the layout and takes the look from the prompt. Long-press or right-click to save them, or take them from <code>${REF_ROOT}/guides/</code> in the repo.</p>
      <div class="guide-grid">${guides.map(g => `
        <figure class="guide ${g.aspect.startsWith('Portrait') ? 'portrait' : ''}" id="guide-${g.id}">
          <img src="guides/${g.file}.png" alt="${esc(g.id + ' layout guide: ' + g.about)}" loading="lazy">
          <figcaption><span class="pid">${g.id}</span> <b>${esc(g.title)}</b><span class="about">${esc(g.about)}</span><span class="used">Used by ${prompts.filter(p => p.attach.includes(g.id)).map(p => p.id).join(', ')}</span><code>${esc(g.file)}.png</code></figcaption>
        </figure>`).join('')}
      </div>
    </div>
    <div class="where">
      <h3>Where the files go</h3>
      <p>Every image has a fixed place in the repo under <code>${REF_ROOT}/</code>, one folder per section. Each card shows its full path.</p>
      <div class="folders">
        <table>
          <thead><tr><th scope="col">Folder</th><th scope="col">What goes there</th></tr></thead>
          <tbody>${sections.map(s => `<tr><td><code>${esc(REF_ROOT)}/${esc(s.folder)}/</code></td><td>${esc(s.name)} <span class="ids">${prompts.filter(p => p.section === s.id).map(p => p.id).join(' ')}</span></td></tr>`).join('')}</tbody>
        </table>
      </div>
      <p><b>Two ways to add them:</b> attach the images in our chat and I'll file them, or open the <a href="${REF_URL}">reference folder on GitHub</a>, go into the folder, and use <b>Add file → Upload files</b>. If a download comes out as <code>.jpg</code> or <code>.webp</code>, keep that extension; the name before it is what matters.</p>
    </div>
    ${toolKeys.map(k => `<div class="notes only-${k}">
      <h3>Working in ${TOOLS[k].name}</h3>
      <ul>${notes[k].map(n => `<li>${esc(n)}</li>`).join('')}</ul>
    </div>`).join('\n    ')}
    <details class="styles">
      <summary>Shared style blocks for <span class="only-chatgpt">ChatGPT</span><span class="only-gemini">Gemini</span> (already in every Copy)</summary>${styleBlocks}
    </details>
  </section>

  <div class="bar">
    <div class="wrap bar-inner">${toolSwitch('tool-bar')}
      <div class="chips" role="toolbar" aria-label="Filter prompts">
        <button class="chip" type="button" data-filter="first" aria-pressed="true">First batch</button>
        <button class="chip" type="button" data-filter="all" aria-pressed="false">All</button>
        ${sectionNav}
      </div>
      <span class="progress" id="progress"><b>0</b> of ${prompts.length} made</span>
    </div>
  </div>

  <div class="wrap">${cards}
  </div>

  <section class="outro wrap" aria-labelledby="build">
    <h2 id="build">What I'll build in Blender</h2>
    <h3>Scenes</h3>
    <ul>${scenes.map(([n, t]) => `<li><b>${esc(n)}.</b> ${esc(t)}</li>`).join('')}</ul>
    <h3>Animations</h3>
    <ul>${animations.map(([n, t]) => `<li><b>${esc(n)}.</b> ${esc(t)}</li>`).join('')}</ul>
    <p style="color: var(--ink-2); margin-top: 1rem">You get <code>.blend</code> scenes, FBX characters and animations ready for Unity, and preview renders and videos along the way.</p>
    <h3>Sending things back</h3>
    <ul>
      <li><b>Images:</b> save each under the name on its card, then attach them in our chat or upload them to their folder under <code>${REF_ROOT}/</code>.</li>
      <li><b>Models:</b> any format Blender opens: <code>.blend</code>, <code>.fbx</code>, <code>.glb</code> or <code>.obj</code>. Metres as units is ideal. A humanoid rig is a bonus; I can rig unrigged characters.</li>
      <li><b>Rights:</b> only send models and images you made or are allowed to use in a commercial game, and tell me any credit a licence asks for.</li>
    </ul>
  </section>
</main>

<script id="prompt-data" type="application/json">${JSON.stringify(payload).replace(/</g, '\\u003c')}</script>
<script>
(function () {
  var data = JSON.parse(document.getElementById('prompt-data').textContent);
  var DONE_KEY = 'fkc-prompts-done';
  var TOOL_KEY = 'fkc-prompts-tool';
  var ids = Object.keys(data.chatgpt.prompts);
  var done = {};
  try { done = JSON.parse(localStorage.getItem(DONE_KEY) || '{}') || {}; } catch (e) { done = {}; }
  var tool = 'chatgpt';
  try { if (localStorage.getItem(TOOL_KEY) === 'gemini') tool = 'gemini'; } catch (e) {}
  if (location.hash === '#gemini' || location.hash === '#chatgpt') tool = location.hash.slice(1);

  function setTool(next) {
    tool = next === 'gemini' ? 'gemini' : 'chatgpt';
    if (tool === 'gemini') document.body.setAttribute('data-tool', 'gemini'); else document.body.removeAttribute('data-tool');
    document.querySelectorAll('[data-tool-choice]').forEach(function (r) { r.checked = r.value === tool; });
    document.querySelectorAll('.fallback').forEach(function (a) { a.hidden = true; });
    try { localStorage.setItem(TOOL_KEY, tool); } catch (e) {}
  }
  document.querySelectorAll('[data-tool-choice]').forEach(function (radio) {
    radio.addEventListener('change', function () { if (radio.checked) setTool(radio.value); });
  });

  function save() { try { localStorage.setItem(DONE_KEY, JSON.stringify(done)); } catch (e) {} }
  function updateProgress() {
    var count = ids.filter(function (id) { return done[id]; }).length;
    document.getElementById('progress').innerHTML = '<b>' + count + '</b> of ' + ids.length + ' made';
  }
  function flash(card, message) {
    var status = card && card.querySelector('.status');
    if (!status) return;
    status.textContent = message;
    clearTimeout(status._t);
    status._t = setTimeout(function () { status.textContent = ''; }, 2200);
  }
  function copyText(text, card, fallbackId, button) {
    function showFallback() {
      var area = fallbackId && document.getElementById(fallbackId);
      if (area) { area.value = text; area.hidden = false; area.focus(); area.select(); flash(card, 'Select all and copy from the box below.'); }
      else if (button) { button.textContent = 'Copy failed'; }
    }
    if (navigator.clipboard && navigator.clipboard.writeText) {
      navigator.clipboard.writeText(text).then(function () {
        flash(card, 'Copied. Paste it into ' + (tool === 'gemini' ? 'Gemini.' : 'ChatGPT.'));
        if (!card && button) { var label = button.textContent; button.textContent = 'Copied'; setTimeout(function () { button.textContent = label; }, 1800); }
      }, showFallback);
    } else { showFallback(); }
  }

  document.querySelectorAll('[data-copy]').forEach(function (button) {
    button.addEventListener('click', function () {
      var id = button.getAttribute('data-copy');
      copyText(data[tool].prompts[id], button.closest('.card'), 'fallback-' + id, button);
    });
  });
  document.querySelectorAll('[data-copy-file]').forEach(function (button) {
    button.addEventListener('click', function () {
      var file = data.files[button.getAttribute('data-id')];
      var kind = button.getAttribute('data-copy-file');
      var text = kind === 'path' ? file.path : file.name;
      var card = button.closest('.card');
      if (!navigator.clipboard || !navigator.clipboard.writeText) { selectPath(card); return; }
      navigator.clipboard.writeText(text).then(function () {
        flash(card, kind === 'path' ? 'Path copied.' : 'File name copied.');
      }, function () { selectPath(card); });
    });
  });
  function selectPath(card) {
    var node = card.querySelector('.path');
    var range = document.createRange();
    range.selectNodeContents(node);
    var sel = window.getSelection();
    sel.removeAllRanges();
    sel.addRange(range);
    flash(card, 'Selected. Copy it from the highlighted path.');
  }
  document.querySelectorAll('[data-copy-style]').forEach(function (button) {
    button.addEventListener('click', function () {
      var parts = button.getAttribute('data-copy-style').split(':');
      copyText(data[parts[0]].styles[parts[1]], null, null, button);
    });
  });
  document.querySelectorAll('[data-done]').forEach(function (box) {
    var id = box.getAttribute('data-done');
    box.checked = !!done[id];
    box.closest('.card').classList.toggle('is-done', box.checked);
    box.addEventListener('change', function () {
      if (box.checked) done[id] = true; else delete done[id];
      box.closest('.card').classList.toggle('is-done', box.checked);
      save();
      updateProgress();
    });
  });

  var chips = document.querySelectorAll('.chip');
  function applyFilter(filter) {
    chips.forEach(function (c) { c.setAttribute('aria-pressed', String(c.getAttribute('data-filter') === filter)); });
    document.querySelectorAll('.card').forEach(function (card) {
      var show = filter === 'all' || (filter === 'first' ? card.getAttribute('data-batch') === 'first' : card.getAttribute('data-section') === filter);
      card.hidden = !show;
    });
    document.querySelectorAll('.group').forEach(function (group) {
      group.hidden = !group.querySelector('.card:not([hidden])');
    });
  }
  chips.forEach(function (chip) { chip.addEventListener('click', function () { applyFilter(chip.getAttribute('data-filter')); }); });
  document.querySelectorAll('.ref').forEach(function (link) {
    link.addEventListener('click', function () {
      var target = document.getElementById(link.getAttribute('href').slice(1));
      if (target && target.hidden) applyFilter('all');
    });
  });
  setTool(tool);
  applyFilter('first');
  updateProgress();
})();
</script>
`;
writeFileSync(join(here, 'sheet.html'), html);
console.log(`Built ${prompts.length} prompts (${firstCount} first batch) for ${toolKeys.map(k => TOOLS[k].name).join(' and ')}: sheet.html, README.md`);
