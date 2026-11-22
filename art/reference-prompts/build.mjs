// Builds the prompt sheet from prompts.json: sheet.html (the interactive page) and README.md (the same prompts as
// plain text). Run with `node art/reference-prompts/build.mjs` after editing prompts.json.
import { readFileSync, writeFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const data = JSON.parse(readFileSync(join(here, 'prompts.json'), 'utf8'));
const { styles, rules, sections, prompts } = data;

const ids = new Set(prompts.map(p => p.id));
for (const p of prompts) {
  if (!styles[p.style]) throw new Error(`${p.id}: unknown style ${p.style}`);
  if (!sections.some(s => s.id === p.section)) throw new Error(`${p.id}: unknown section ${p.section}`);
  for (const a of p.attach) if (!ids.has(a)) throw new Error(`${p.id}: attaches unknown ${a}`);
  const order = prompts.findIndex(q => q.id === p.id);
  for (const a of p.attach) {
    if (prompts.findIndex(q => q.id === a) > order) throw new Error(`${p.id}: attaches ${a}, which comes later`);
  }
}

const fullPrompt = p => `${p.prompt}\n\n${styles[p.style].text}\n\n${rules}`;
const firstCount = prompts.filter(p => p.batch === 'first').length;

const scenes = [
  ['The stare-down arena', 'The first playable world: the floating night arena, its stands, crowd, sky and lighting, with the phone cameras for kicker, keeper and the stare-down close-up.'],
  ['The free-kick set piece', 'The same arena with a wall at the 9.15 m spray line and the referee.'],
  ['Two more eras', 'The 1970s sun-bleached afternoon and the 1990s floodlit neon night, each lit and graded as its own world.'],
  ['Classics scenes', 'Clifftop, storm, frozen lake, monsoon and above the clouds, one per scenario card.'],
];
const animations = [
  ['Kicker', 'Ready idle, run-up, stutter, side-foot, driven knuckleball, curler, trivela, chip, two celebrations, a miss.'],
  ['Keeper', 'Ready stance, shuffle, low, middle and top-corner dives to both sides, staying big, catch, smother, fingertip save, punch, celebration, despair.'],
  ['Wall', 'Line up, jump, brace, the lying man.'],
  ['Ball and effects', 'Light ribbons that follow flight paths exported from the game\'s physics, the net bursting into light, crowd waves.'],
];

const esc = s => String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');

// ---------- README.md ----------
const md = [];
md.push('# Reference art prompts', '');
md.push('Image prompts to generate in ChatGPT. The images come back to me as references for building the Blender scenes and animations.', '');
md.push(`There are ${prompts.length} prompts. Make the ${firstCount} in the first batch before the others.`, '');
md.push('Edit `prompts.json`, then run `node art/reference-prompts/build.mjs` to rebuild this file and `sheet.html`.', '');
md.push('## How to use them', '');
md.push('1. Generate **A01** first, in a new ChatGPT chat. Make three or four variations and keep the one you like best.');
md.push('2. For every later prompt, attach the images listed under **Attach**. Always include A01.');
md.push('3. Give each character its own chat, so the face and kit stay the same across that character\'s sheets.');
md.push('4. Save each image with its ID as the file name, e.g. `C01.png` or `C01-v2.png`, and send them to me.', '');
md.push('Each prompt below is complete: the prompt, then its style block, then the originality rules. Copy the whole block.', '');
for (const section of sections) {
  md.push(`## ${section.name}`, '', section.blurb, '');
  for (const p of prompts.filter(q => q.section === section.id)) {
    md.push(`### ${p.id} · ${p.title}`, '');
    md.push(`- **Batch:** ${p.batch === 'first' ? 'first' : 'later'}`);
    md.push(`- **Image shape:** ${p.aspect}`);
    md.push(`- **Attach:** ${p.attach.length ? p.attach.join(', ') : 'nothing'}`);
    md.push(`- **I'll build:** ${p.builds}`);
    if (p.tip) md.push(`- **Tip:** ${p.tip}`);
    md.push('', '```text', fullPrompt(p), '```', '');
  }
}
md.push('## What I\'ll build in Blender', '');
md.push('**Scenes**', '');
for (const [name, text] of scenes) md.push(`- **${name}.** ${text}`);
md.push('', '**Animations**', '');
for (const [name, text] of animations) md.push(`- **${name}.** ${text}`);
md.push('', 'You get `.blend` scenes, FBX characters and animations ready for Unity, and preview renders and videos along the way.', '');
md.push('## Sending things back', '');
md.push('- **Images:** name each file with its prompt ID (`A01.png`; `A01-v2.png` for alternatives) and attach them in our chat.');
md.push('- **Models:** any format Blender opens: `.blend`, `.fbx`, `.glb`/`.gltf` or `.obj`. Metres as units is ideal. A humanoid rig is a bonus; I can rig unrigged characters.');
md.push('- **Rights:** only send models and images you made or are allowed to use in a commercial game, and tell me any credit a licence requires.');
writeFileSync(join(here, 'README.md'), md.join('\n') + '\n');

// ---------- sheet.html ----------
const sectionNav = sections.map(s => `<button class="chip" type="button" data-filter="${s.id}">${esc(s.name)}</button>`).join('');
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
          <div><dt>Attach</dt><dd>${p.attach.length ? p.attach.map(a => `<a class="ref" href="#${a}">${a}</a>`).join(' ') : '<span class="none">Nothing</span>'}</dd></div>
        </dl>
        <p class="builds"><span>I'll build</span>${esc(p.builds)}</p>
        <p class="prompt">${esc(p.prompt)}</p>
        <p class="appended">Copies with the ${esc(styles[p.style].name.toLowerCase())} and the originality rules.</p>
        ${p.tip ? `<p class="tip"><span>Tip</span>${esc(p.tip)}</p>` : ''}
        <div class="actions">
          <button class="copy" type="button" data-copy="${p.id}">Copy prompt</button>
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

const styleBlocks = Object.entries(styles).map(([key, s]) => `
        <div class="block">
          <div class="block-head"><h3>${esc(s.name)}</h3><span>${esc(s.use)}</span>
            <button class="copy small" type="button" data-copy-style="${key}">Copy</button></div>
          <p>${esc(s.text)}</p>
        </div>`).join('') + `
        <div class="block">
          <div class="block-head"><h3>Originality rules</h3><span>Every prompt</span>
            <button class="copy small" type="button" data-copy-style="rules">Copy</button></div>
          <p>${esc(rules)}</p>
        </div>`;

const html = `<title>Free Kick Classics Prompts</title>
<meta name="claude:viewport-fit-sides" content="cover">
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Bricolage+Grotesque:opsz,wght@12..96,500;12..96,700&family=Inter:wght@400;500;600&family=JetBrains+Mono:wght@500&display=swap">
<style>
  /* Layout: one reading column, like an art-direction booklet read at dusk. Deliberately dark-only: it is the night arena's palette. */
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
    --aurora-green: #3fd39a;
    --violet: #9b84ff;
    --amber: #e8955a;
    --display: "Bricolage Grotesque", "Avenir Next", "Segoe UI", system-ui, sans-serif;
    --body: "Inter", system-ui, -apple-system, "Segoe UI", sans-serif;
    --mono: "JetBrains Mono", ui-monospace, "SF Mono", Menlo, monospace;
  }
  * { box-sizing: border-box; }
  body { background: var(--night); color: var(--ink); font-family: var(--body); font-size: 15px; line-height: 1.6; }
  a { color: var(--gold); }
  .wrap { max-width: 880px; margin: 0 auto; padding-left: max(16px, env(safe-area-inset-left, 0px)); padding-right: max(16px, env(safe-area-inset-right, 0px)); }

  .hero { margin-top: calc(-1 * env(safe-area-inset-top, 0px)); padding-top: calc(env(safe-area-inset-top, 0px) + 3.5rem); padding-bottom: 2.75rem;
    background:
      radial-gradient(60% 90% at 18% 0%, rgba(63, 211, 154, 0.28), transparent 70%),
      radial-gradient(55% 80% at 70% 10%, rgba(155, 132, 255, 0.30), transparent 70%),
      radial-gradient(40% 60% at 95% 60%, rgba(234, 185, 95, 0.16), transparent 70%),
      linear-gradient(180deg, #151332 0%, var(--night) 100%);
    border-bottom: 1px solid var(--rule); }
  .eyebrow { font-family: var(--mono); font-size: 12px; letter-spacing: 0.08em; text-transform: uppercase; color: var(--gold); margin: 0 0 0.75rem; }
  h1 { font-family: var(--display); font-weight: 700; font-size: clamp(2.1rem, 5.5vw, 3.4rem); line-height: 1.04; letter-spacing: -0.02em; margin: 0 0 1rem; text-wrap: balance; max-width: 16ch; }
  .lead { font-size: 1.08rem; color: var(--ink-2); max-width: 62ch; margin: 0 0 1.5rem; }
  .facts { display: flex; flex-wrap: wrap; gap: 0.5rem 1.5rem; font-family: var(--mono); font-size: 13px; color: var(--ink-2); margin: 0; padding: 0; list-style: none; }
  .facts b { color: var(--ink); font-weight: 500; }

  main section.intro { padding-block: 2.5rem 0.5rem; }
  h2 { font-family: var(--display); font-weight: 700; font-size: 1.6rem; letter-spacing: -0.01em; margin: 0 0 0.5rem; text-wrap: balance; }
  .intro > p { color: var(--ink-2); max-width: 65ch; margin: 0 0 1.25rem; }
  .pillars { display: grid; gap: 0.9rem; margin: 0 0 1.75rem; padding: 0; list-style: none; }
  .pillars li { display: grid; grid-template-columns: 9.5rem 1fr; gap: 1rem; padding-bottom: 0.9rem; border-bottom: 1px solid var(--rule); }
  .pillars strong { font-family: var(--display); font-weight: 500; font-size: 1.05rem; color: var(--ink); }
  .pillars span { color: var(--ink-2); }
  .eras { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 0.75rem; }
  .era { border-radius: 10px; padding: 0.9rem 1rem 1rem; min-height: 8.5rem; display: flex; flex-direction: column; justify-content: flex-end; border: 1px solid rgba(255,255,255,0.08); }
  .era h3 { font-family: var(--display); font-weight: 700; font-size: 1.05rem; margin: 0; color: #fff; text-shadow: 0 1px 8px rgba(0,0,0,0.45); }
  .era p { margin: 0.2rem 0 0; font-size: 13px; line-height: 1.4; color: rgba(255,255,255,0.88); text-shadow: 0 1px 6px rgba(0,0,0,0.5); }
  .era.seventies { background: linear-gradient(160deg, #e7c38c 0%, #c98a4e 45%, #6f4a35 100%); }
  .era.nineties { background: linear-gradient(160deg, #ff4fb3 0%, #6b2fc2 50%, #0f8f9c 100%); }
  .era.modern { background: radial-gradient(80% 70% at 30% 20%, #3fd39a 0%, transparent 60%), radial-gradient(70% 70% at 80% 30%, #9b84ff 0%, transparent 65%), linear-gradient(180deg, #1f1b52, #0d0c1f); }
  .era .tag { font-family: var(--mono); font-size: 11px; letter-spacing: 0.06em; text-transform: uppercase; color: rgba(255,255,255,0.85); margin-bottom: auto; }

  ol.steps { counter-reset: step; list-style: none; margin: 0 0 1.5rem; padding: 0; display: grid; gap: 0.75rem; }
  ol.steps li { counter-increment: step; display: grid; grid-template-columns: 2rem 1fr; gap: 0.75rem; align-items: start; color: var(--ink-2); }
  ol.steps li::before { content: counter(step); font-family: var(--mono); font-size: 13px; color: var(--night); background: var(--gold); width: 1.6rem; height: 1.6rem; border-radius: 50%; display: grid; place-items: center; margin-top: 0.1rem; }
  ol.steps b { color: var(--ink); font-weight: 600; }

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
  .bar-inner { display: flex; align-items: center; gap: 0.75rem; padding-block: 0.65rem; }
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
  .card { background: var(--deep); border: 1px solid var(--rule); border-radius: 12px; padding: 1.1rem 1.2rem 1rem; margin-bottom: 0.9rem; scroll-margin-top: 5rem; }
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
  .none { color: var(--muted); }
  .builds, .tip { margin: 0 0 0.6rem; font-size: 14px; color: var(--ink-2); }
  .builds span, .tip span { font-family: var(--mono); font-size: 11px; letter-spacing: 0.06em; text-transform: uppercase; color: var(--muted); margin-right: 0.6rem; }
  .prompt { margin: 0 0 0.4rem; padding: 0.85rem 1rem; background: var(--raised); border-radius: 8px; color: var(--ink); font-size: 15px; }
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
  .outro code { font-family: var(--mono); font-size: 13px; color: var(--gold); }

  @media (max-width: 640px) {
    .pillars li { grid-template-columns: 1fr; gap: 0.15rem; }
    .eras { grid-template-columns: 1fr; }
    .era { min-height: 6.5rem; }
    .card { padding: 1rem; }
  }
  @media (prefers-reduced-motion: no-preference) {
    .card { transition: border-color 0.2s ease; }
  }
</style>

<header class="hero">
  <div class="wrap">
    <p class="eyebrow">Free Kick Classics · reference art</p>
    <h1>Image prompts for the Blender build</h1>
    <p class="lead">Generate these in ChatGPT and send the images back. I'll model, rig, light and animate the game's scenes from them in Blender, with every ball flight taken from the game's own physics.</p>
    <ul class="facts">
      <li><b>${prompts.length}</b> prompts</li>
      <li><b>${firstCount}</b> in the first batch</li>
      <li>Start with <b>A01</b></li>
    </ul>
  </div>
</header>

<main>
  <section class="intro wrap" aria-labelledby="look">
    <h2 id="look">The look</h2>
    <p>Every kick is a held breath stretched into a dream. The world around the pitch is painterly, luminous and a little impossible. The goal, the ball and the two players stay crisp, because the game is still about reading your friend.</p>
    <ul class="pillars">
      <li><strong>Painted, not photographed</strong><span>Stylized 3D with hand-painted textures, soft volumetric light and visible brushwork in skies and crowds.</span></li>
      <li><strong>Light shows the physics</strong><span>The ball leaves a ribbon of light whose shape is the real curl, dip or knuckle wobble from the flight model.</span></li>
      <li><strong>Faces carry the duel</strong><span>Expressive faces and close-ups for the stare-down: the bluff, the read, the relief.</span></li>
      <li><strong>Worlds, not filters</strong><span>Each era and each Classics card is its own place: its weather, its light, its crowd.</span></li>
      <li><strong>Readable first</strong><span>During a kick the world dims around the goal; the ball, frame and players always hold the strongest contrast.</span></li>
    </ul>
    <div class="eras" role="list">
      <div class="era seventies" role="listitem"><span class="tag">Heavy leather ball</span><h3>1970s</h3><p>Sun-bleached afternoon: amber haze, film grain, packed terraces.</p></div>
      <div class="era nineties" role="listitem"><span class="tag">Big-bending glossy ball</span><h3>1990s</h3><p>Floodlight neon: chrome, magenta and teal flares, wet grass.</p></div>
      <div class="era modern" role="listitem"><span class="tag">Light knuckling ball</span><h3>Modern</h3><p>The dream arena: a pitch on a mirror lake under aurora. First to build.</p></div>
    </div>
  </section>

  <section class="intro wrap" aria-labelledby="how">
    <h2 id="how">How to use this sheet</h2>
    <ol class="steps">
      <li><span><b>Make A01 first</b> in a new ChatGPT chat. Ask for three or four variations and keep the one you love; it sets the look for everything else.</span></li>
      <li><span><b>Attach what each card lists.</b> Always include A01, plus the character sheets a pose or face prompt names.</span></li>
      <li><span><b>One chat per character.</b> The kicker, keeper and wall each get their own chat, so faces and kits stay the same from sheet to sheet.</span></li>
      <li><span><b>Name files by ID</b>, like <code>C01.png</code> or <code>C01-v2.png</code>, tick “Made it”, and send the images to me in our chat.</span></li>
    </ol>
    <details class="styles">
      <summary>Shared style blocks (already included in every Copy)</summary>
      <div class="blocks">${styleBlocks}
      </div>
    </details>
  </section>

  <div class="bar">
    <div class="wrap bar-inner">
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
      <li><b>Images:</b> name each file with its prompt ID and attach them in our chat. Send alternatives as <code>A01-v2.png</code>.</li>
      <li><b>Models:</b> any format Blender opens: <code>.blend</code>, <code>.fbx</code>, <code>.glb</code> or <code>.obj</code>. Metres as units is ideal. A humanoid rig is a bonus; I can rig unrigged characters.</li>
      <li><b>Rights:</b> only send models and images you made or are allowed to use in a commercial game, and tell me any credit a licence asks for.</li>
    </ul>
  </section>
</main>

<script id="prompt-data" type="application/json">${JSON.stringify({ styles, rules, prompts: prompts.map(p => ({ id: p.id, text: fullPrompt(p) })) }).replace(/</g, '\\u003c')}</script>
<script>
(function () {
  var data = JSON.parse(document.getElementById('prompt-data').textContent);
  var full = {};
  data.prompts.forEach(function (p) { full[p.id] = p.text; });
  var styleText = { rules: data.rules };
  Object.keys(data.styles).forEach(function (k) { styleText[k] = data.styles[k].text; });
  var KEY = 'fkc-prompts-done';
  var done = {};
  try { done = JSON.parse(localStorage.getItem(KEY) || '{}') || {}; } catch (e) { done = {}; }

  function save() { try { localStorage.setItem(KEY, JSON.stringify(done)); } catch (e) {} }
  function updateProgress() {
    var count = Object.keys(full).filter(function (id) { return done[id]; }).length;
    document.getElementById('progress').innerHTML = '<b>' + count + '</b> of ' + Object.keys(full).length + ' made';
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
        flash(card, 'Copied. Paste it into ChatGPT.');
        if (!card && button) { var label = button.textContent; button.textContent = 'Copied'; setTimeout(function () { button.textContent = label; }, 1800); }
      }, showFallback);
    } else { showFallback(); }
  }

  document.querySelectorAll('[data-copy]').forEach(function (button) {
    button.addEventListener('click', function () {
      var id = button.getAttribute('data-copy');
      copyText(full[id], button.closest('.card'), 'fallback-' + id, button);
    });
  });
  document.querySelectorAll('[data-copy-style]').forEach(function (button) {
    button.addEventListener('click', function () { copyText(styleText[button.getAttribute('data-copy-style')], null, null, button); });
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
  applyFilter('first');
  updateProgress();
})();
</script>
`;
writeFileSync(join(here, 'sheet.html'), html);
console.log(`Built ${prompts.length} prompts (${firstCount} first batch): sheet.html, README.md`);
