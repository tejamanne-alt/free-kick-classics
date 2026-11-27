# Reference art prompts

Image prompts to generate in ChatGPT or Gemini. The images come back to me as references for building the Blender scenes and animations.

There are 43 prompts, each in a ChatGPT and a Gemini version. Make the 26 in the first batch before the others.

Edit `prompts.json`, then run `node art/reference-prompts/build.mjs` to rebuild this file and `sheet.html`.

## How to use them

1. Generate **A01** first, in a new chat. Make three or four variations and keep the one you like best.
2. For every later prompt, attach exactly the images listed under **Attach**, in that order.
3. Give each character its own chat, so the face and kit stay the same across that character's sheets.
4. Save each image under the name given below, e.g. `C01-kicker-turnaround.png`. Alternatives get `-v2`, `-v3`.

## Where the files go

Every image has a fixed place under `art/reference/`, one folder per section:

| Folder | What goes there |
| --- | --- |
| `art/reference/style/` | Start here (A01, A02, A03) |
| `art/reference/characters/` | Characters (C01, C02, C03, C04, C05, C06, C07) |
| `art/reference/poses/` | Animation poses (P01, P02, P03, P04, P05, P06, P07, P08, P09, P10) |
| `art/reference/props/` | Ball, goal and pitch (B01, B02, B03, B04) |
| `art/reference/arena/` | Arena and cameras (W01, W02, W03, W04) |
| `art/reference/moments/` | Moments (M01, M02, M03, M04) |
| `art/reference/textures/` | Textures (T01, T02, T03, T04) |
| `art/reference/worlds/` | Eras and Classics (E01, E02, S01, S02, S03, S04, S05) |

Two ways to get them there: attach them in our chat and I'll file them, or open the folder on GitHub and use **Add file → Upload files**. If a download comes out as `.jpg` or `.webp`, keep that extension; the name before it is what matters.

**ChatGPT**

- Each prompt opens with the image shape, and ChatGPT picks landscape, portrait or square from it.
- Attach the listed images, then paste the prompt into the same message.
- To fix something, reply in the same chat, for example: “Same sheet, but flat orthographic views with no shadows.”

**Gemini**

- Upload the listed images first, in the order the card shows. The prompt names them in that order.
- Gemini follows descriptions of what to include better than lists of what to avoid, so these prompts describe clean, plain surfaces instead of banning logos one by one.
- Refine in the same chat, for example: “Keep everything the same, but make the background plain light grey.”
- If an image comes back in the wrong shape, reply: “Recompose this as a 3:2 landscape image.”

Each prompt below is complete, with its style block and originality rules. Copy the whole block for the tool you use.

## Start here

The style keyframe sets the look for everything else. Make it first, pick the variation you love, and attach it to every later prompt.

### A01 · Style keyframe: the stare-down

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/style/A01-style-keyframe.png`
- **Attach, in order:** nothing
- **I'll build:** The master look: lighting, colour grade, fog and sky for every scene.
- **Tip:** Generate three or four variations and keep the one you love. Attach it to every prompt after this one.

**ChatGPT**

```text
Make a landscape image (3:2). Cinematic wide frame of a penalty kick at golden hour in an original modern football stadium built on a harbourfront. In the foreground a kicker in a deep teal kit stands behind the ball on the penalty spot, seen from behind and slightly to one side. On the goal line the keeper, in a warm amber kit, crouches with his eyes locked on the kicker. Behind the goal rises a steep two-tier stand packed with fans in scarves. The far end of the stadium is open, framing the sea and a low sun that throws long shadows across freshly mown, striped turf. The floodlights are just flickering on, light haze hangs over the pitch, and a few strands of coloured flare smoke drift from the stand. Shadows break into halftone dots, every edge carries a hand-inked line, and the far stand dissolves into offset colour, like a printed comic page come to life. The mood is a held breath just before the strike.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format.

Cinematic wide frame of a penalty kick at golden hour in an original modern football stadium built on a harbourfront. In the foreground a kicker in a deep teal kit stands behind the ball on the penalty spot, seen from behind and slightly to one side. On the goal line the keeper, in a warm amber kit, crouches with his eyes locked on the kicker. Behind the goal rises a steep two-tier stand packed with fans in scarves. The far end of the stadium is open, framing the sea and a low sun that throws long shadows across freshly mown, striped turf. The floodlights are just flickering on, light haze hangs over the pitch, and a few strands of coloured flare smoke drift from the stand. Shadows break into halftone dots, every edge carries a hand-inked line, and the far stand dissolves into offset colour, like a printed comic page come to life. The mood is a held breath just before the strike.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### A02 · Palette, materials and print patterns

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/style/A02-palette-materials.png`
- **Attach, in order:** A01
- **I'll build:** The Blender material library, the colour values every scene shares, and the halftone and ink shaders.

**ChatGPT**

```text
Make a landscape image (3:2). A tidy art-direction board on a dark charcoal background, using the colours of the attached keyframe. Across the top, a row of eight plain colour swatches: sunset gold, harbour blue, deep pitch green, chalk-line white, teal, amber, weathered concrete grey and floodlight white. Below them, six material spheres in a row, each drawn in the comic style with ink outlines and halftone shading: striped turf, stitched ball leather, smooth modern ball synthetic, kit fabric, goal-net nylon, and weathered stadium concrete. Along the bottom, four square pattern swatches: fine halftone dots, coarse Ben-Day dots, hand-inked cross-hatching, and offset colour misregistration.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A tidy art-direction board on a dark charcoal background, using the colours of the attached keyframe. Across the top, a row of eight plain colour swatches: sunset gold, harbour blue, deep pitch green, chalk-line white, teal, amber, weathered concrete grey and floodlight white. Below them, six material spheres in a row, each drawn in the comic style with ink outlines and halftone shading: striped turf, stitched ball leather, smooth modern ball synthetic, kit fabric, goal-net nylon, and weathered stadium concrete. Along the bottom, four square pattern swatches: fine halftone dots, coarse Ben-Day dots, hand-inked cross-hatching, and offset colour misregistration.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### A03 · Replay tracers: four inked ball flights

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/style/A03-replay-tracers.png`
- **Attach, in order:** A01
- **I'll build:** The replay trajectory effect. In Blender the inked lines follow real flight paths exported from the game's own physics.

**ChatGPT**

```text
Make a landscape image (3:2). Four comic panels side by side, each a side-on replay of a football's flight in a stadium, with the path drawn as a bold inked curve, speed lines and a fading halftone trail. Panel one: a smooth bending curve for a curling shot. Panel two: an arc that dips sharply at the end for a topspin shot. Panel three: a nearly straight path that wobbles only in its last third, for a knuckleball, with jittery ink strokes. Panel four: a high, gentle lob for a chip.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Four comic panels side by side, each a side-on replay of a football's flight in a stadium, with the path drawn as a bold inked curve, speed lines and a fading halftone trail. Panel one: a smooth bending curve for a curling shot. Panel two: an arc that dips sharply at the end for a topspin shot. Panel three: a nearly straight path that wobbles only in its last third, for a knuckleball, with jittery ink strokes. Panel four: a high, gentle lob for a chip.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## Characters

Turnaround sheets I model and rig from. Keep each character in its own chat so the face and kit stay consistent.

### C01 · Kicker turnaround

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C01-kicker-turnaround.png`
- **Attach, in order:** A01
- **I'll build:** The kicker model, rigged as a humanoid so animations can be shared and exported to Unity.
- **Tip:** If the views come back with perspective or shadows, reply in the same chat: “Same sheet, but flat orthographic views, evenly lit, every view the same height.”

**ChatGPT**

```text
Make a landscape image (3:2). Character turnaround model sheet of the kicker from the attached keyframe: an original young professional footballer with an athletic build and grounded, slightly stylized proportions, short curly dark hair, warm brown skin and determined eyes. Deep teal kit with a subtle tonal pattern across the shirt, white shorts, teal socks and gold boots. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Character turnaround model sheet of the kicker from the attached keyframe: an original young professional footballer with an athletic build and grounded, slightly stylized proportions, short curly dark hair, warm brown skin and determined eyes. Deep teal kit with a subtle tonal pattern across the shirt, white shorts, teal socks and gold boots. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### C02 · Kicker face and expressions

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C02-kicker-faces.png`
- **Attach, in order:** A01, C01
- **I'll build:** Head modelling and the facial shape keys for stare-down close-ups.

**ChatGPT**

```text
Make a landscape image (3:2). Head and face reference sheet for the kicker in the attached turnaround. Top row: the neutral head from the front, three-quarter and side. Bottom row: four expressions from the front: calm focus, a sly bluffing half-smile, wide-eyed joy after scoring, and frustration after a miss. Shoulders up, with the same face shape and hair in every view.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Head and face reference sheet for the kicker in the attached turnaround. Top row: the neutral head from the front, three-quarter and side. Bottom row: four expressions from the front: calm focus, a sly bluffing half-smile, wide-eyed joy after scoring, and frustration after a miss. Shoulders up, with the same face shape and hair in every view.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### C03 · Keeper turnaround

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C03-keeper-turnaround.png`
- **Attach, in order:** A01
- **I'll build:** The keeper model, on the same humanoid rig as the kicker.
- **Tip:** Start a new chat for the keeper so the kicker's features don't bleed into him.

**ChatGPT**

```text
Make a landscape image (3:2). Character turnaround model sheet of the keeper from the attached keyframe: an original goalkeeper, tall with long arms and large hands, grounded, slightly stylized athletic proportions, shaved head with a short beard, light tan skin and calm, intense eyes. Long-sleeved warm amber goalkeeper kit with darker amber elbow padding and a subtle tonal pattern, black shorts, amber socks, and well-worn goalkeeper gloves. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Character turnaround model sheet of the keeper from the attached keyframe: an original goalkeeper, tall with long arms and large hands, grounded, slightly stylized athletic proportions, shaved head with a short beard, light tan skin and calm, intense eyes. Long-sleeved warm amber goalkeeper kit with darker amber elbow padding and a subtle tonal pattern, black shorts, amber socks, and well-worn goalkeeper gloves. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### C04 · Keeper face and gloves

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C04-keeper-faces-gloves.png`
- **Attach, in order:** A01, C03
- **I'll build:** The keeper's head, facial shape keys and glove model.

**ChatGPT**

```text
Make a landscape image (3:2). Face and glove reference sheet for the keeper in the attached turnaround. Top row: the neutral head from the front, three-quarter and side. Bottom row: four expressions from the front: reading the kicker with narrowed eyes, a taunting grin, a roaring celebration, and quiet despair. On the right, a close study of one goalkeeper glove from the palm side and from the back.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Face and glove reference sheet for the keeper in the attached turnaround. Top row: the neutral head from the front, three-quarter and side. Bottom row: four expressions from the front: reading the kicker with narrowed eyes, a taunting grin, a roaring celebration, and quiet despair. On the right, a close study of one goalkeeper glove from the palm side and from the back.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### C05 · Wall players

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C05-wall-players.png`
- **Attach, in order:** A01
- **I'll build:** Three body types for the defensive wall in free kicks.

**ChatGPT**

```text
Make a landscape image (3:2). Turnaround sheet of three original outfield players who will form a defensive wall, all in the same deep crimson kit with a faint diagonal pattern: one broad and short, one tall and lean, one of medium build, each with a different hairstyle and skin tone. Show each player from the front and from the left side, standing in an A-pose, so there are six full-body views in one row.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Turnaround sheet of three original outfield players who will form a defensive wall, all in the same deep crimson kit with a faint diagonal pattern: one broad and short, one tall and lean, one of medium build, each with a different hairstyle and skin tone. Show each player from the front and from the left side, standing in an A-pose, so there are six full-body views in one row.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### C06 · Kit flat-lays

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C06-kit-flat-lays.png`
- **Attach, in order:** A01, C01, C03
- **I'll build:** Texture painting for the kits and the alternate colours.

**ChatGPT**

```text
Make a landscape image (3:2). Flat-lay kit design sheet seen straight down: four complete football kits, each laid out neatly with shirt, shorts and socks and shown from the front and the back. The kicker's teal and gold kit from the kicker turnaround, an alternate kit in chalk white and violet, the keeper's amber kit from the keeper turnaround, and the wall team's deep crimson kit. Fabric weave and pattern detail clearly visible.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01); third, the keeper turnaround (C03).

Flat-lay kit design sheet seen straight down: four complete football kits, each laid out neatly with shirt, shorts and socks and shown from the front and the back. The kicker's teal and gold kit from the kicker turnaround, an alternate kit in chalk white and violet, the keeper's amber kit from the keeper turnaround, and the wall team's deep crimson kit. Fabric weave and pattern detail clearly visible.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### C07 · The referee

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C07-referee.png`
- **Attach, in order:** A01
- **I'll build:** The referee who sprays the 9.15 m line and pushes the wall back.

**ChatGPT**

```text
Make a landscape image (3:2). Character turnaround model sheet of an original referee with a calm, authoritative presence: a black kit with subtle gold trim, a whistle on a cord, and a can of vanishing spray on his belt. Three full-body views side by side: front, left side and back, in an A-pose.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Character turnaround model sheet of an original referee with a calm, authoritative presence: a black kit with subtle gold trim, a whistle on a cord, and a can of vanishing spray on his belt. Three full-body views side by side: front, left side and back, in an A-pose.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## Animation poses

Key-pose sheets I turn into keyframed animations. Attach the character's turnaround so the poses match the model.

### P01 · Run-up and side-foot strike

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P01-run-up-side-foot.png`
- **Attach, in order:** A01, C01
- **I'll build:** The 1.5-second run-up and the placed shot.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the kicker in the attached turnaround taking a penalty, seen from the side. Eight poses in a single row from left to right, evenly spaced: standing ready behind the ball; first step; mid-stride; long last stride with the planted foot landing beside the ball; kicking leg swinging back; side-foot contact with the ball; follow-through; landing and looking up.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround taking a penalty, seen from the side. Eight poses in a single row from left to right, evenly spaced: standing ready behind the ball; first step; mid-stride; long last stride with the planted foot landing beside the ball; kicking leg swinging back; side-foot contact with the ball; follow-through; landing and looking up.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P02 · Driven knuckleball strike

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P02-driven-knuckleball.png`
- **Attach, in order:** A01, C01
- **I'll build:** The laces strike that feeds the knuckleball.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the kicker in the attached turnaround striking a driven knuckleball, seen from the side. Eight poses in a single row from left to right, evenly spaced: ready; fast approach; planted foot; kicking leg cocked high; laces striking through the middle of the ball with the ankle locked; short, stabbing follow-through; landing; watching the ball fly.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround striking a driven knuckleball, seen from the side. Eight poses in a single row from left to right, evenly spaced: ready; fast approach; planted foot; kicking leg cocked high; laces striking through the middle of the ball with the ankle locked; short, stabbing follow-through; landing; watching the ball fly.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P03 · Curler and trivela

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P03-curler-trivela.png`
- **Attach, in order:** A01, C01
- **I'll build:** The inside-of-the-foot curler and the outside-of-the-foot trivela.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the kicker in the attached turnaround, seen from the side, in two rows. Top row, four poses of an inside-of-the-foot curler, with the body wrapping around the ball. Bottom row, four poses of an outside-of-the-foot trivela, with the kicking leg swinging across the body.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround, seen from the side, in two rows. Top row, four poses of an inside-of-the-foot curler, with the body wrapping around the ball. Bottom row, four poses of an outside-of-the-foot trivela, with the kicking leg swinging across the body.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P04 · The chip

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P04-chip.png`
- **Attach, in order:** A01, C01
- **I'll build:** The soft chip that punishes an early dive.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the kicker in the attached turnaround playing a soft chip down the middle, seen from the side. Six poses in a single row from left to right: ready; approach, slowing; planted foot; kicking leg swinging gently; foot scooping under the ball; a cheeky follow-through with the weight leaning back.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround playing a soft chip down the middle, seen from the side. Six poses in a single row from left to right: ready; approach, slowing; planted foot; kicking leg swinging gently; foot scooping under the ball; a cheeky follow-through with the weight leaning back.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P05 · Stutter run-up

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P05-stutter.png`
- **Attach, in order:** A01, C01
- **I'll build:** The stutter bluff.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the kicker in the attached turnaround doing a stutter run-up, seen from the side. Six poses in a single row from left to right: approach; a sudden hop and pause mid-run with the arms out for balance; a second small hop; the long final stride; the strike; follow-through.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround doing a stutter run-up, seen from the side. Six poses in a single row from left to right: approach; a sudden hop and pause mid-run with the arms out for balance; a second small hop; the long final stride; the strike; follow-through.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P06 · Keeper ready and shuffle

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P06-keeper-ready-shuffle.png`
- **Attach, in order:** A01, C03
- **I'll build:** The keeper's idle stance and the shuffle bluff on the line.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the keeper in the attached turnaround, seen from the front. Six poses in a single row from left to right: crouched ready stance on the goal line; shuffle step to his left; shuffle step to his right; arms spread wide to look big; bouncing on his toes; set position a moment before the dive.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Animation key-pose sheet of the keeper in the attached turnaround, seen from the front. Six poses in a single row from left to right: crouched ready stance on the goal line; shuffle step to his left; shuffle step to his right; arms spread wide to look big; bouncing on his toes; set position a moment before the dive.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P07 · Keeper dives: low, middle, top corner

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P07-keeper-dives.png`
- **Attach, in order:** A01, C03
- **I'll build:** The dives, mirrored in Blender for the other side. Early and late dives are the same poses with different timing.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the keeper in the attached turnaround, seen from the front as the kicker sees him, in three rows of four poses, all diving to the same side. Top row, a low dive to the bottom corner. Middle row, a dive at waist height. Bottom row, a full-stretch leap to the top corner. Each row runs from left to right: push-off, take-off, full stretch at the height of the ball, landing.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Animation key-pose sheet of the keeper in the attached turnaround, seen from the front as the kicker sees him, in three rows of four poses, all diving to the same side. Top row, a low dive to the bottom corner. Middle row, a dive at waist height. Bottom row, a full-stretch leap to the top corner. Each row runs from left to right: push-off, take-off, full stretch at the height of the ball, landing.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P08 · Stay big, catch and parry

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P08-keeper-saves.png`
- **Attach, in order:** A01, C03
- **I'll build:** Staying big, catches and parries.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the keeper in the attached turnaround, seen from the front. Six poses in a single row from left to right: staying big in a star shape with arms and legs spread; catching a ball against his chest; smothering a low ball on the ground; a one-handed fingertip tip over the bar; a two-fisted punch; getting back up from the ground.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Animation key-pose sheet of the keeper in the attached turnaround, seen from the front. Six poses in a single row from left to right: staying big in a star shape with arms and legs spread; catching a ball against his chest; smothering a low ball on the ground; a one-handed fingertip tip over the bar; a two-fisted punch; getting back up from the ground.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P09 · The wall

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P09-wall-poses.png`
- **Attach, in order:** A01, C05
- **I'll build:** Wall set-up, jump, brace and the lying man for Wall Wars and the Classics cards.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet of the wall players in the attached sheet, seen from the side. Eight poses in a single row from left to right: lining up shoulder to shoulder; hands covering in the set position; crouching before the jump; jumping together with arms down; landing; one player staying grounded to block a low shot; one player lying flat on the ground behind the wall; turning to watch the ball.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the wall players sheet (C05).

Animation key-pose sheet of the wall players in the attached sheet, seen from the side. Eight poses in a single row from left to right: lining up shoulder to shoulder; hands covering in the set position; crouching before the jump; jumping together with arms down; landing; one player staying grounded to block a low shot; one player lying flat on the ground behind the wall; turning to watch the ball.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### P10 · Celebrations and dejection

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/poses/P10-celebrations.png`
- **Attach, in order:** A01, C01, C03
- **I'll build:** The after-kick reactions for both players.

**ChatGPT**

```text
Make a landscape image (3:2). Animation key-pose sheet in two rows. Top row, the kicker from the kicker turnaround, four poses: a knee slide, an arms-wide aeroplane run, a finger to the lips, and hands on head after a miss. Bottom row, the keeper from the keeper turnaround, four poses: a roaring fist pump, pointing to the crowd, lying face down after conceding, and standing with hands on hips.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01); third, the keeper turnaround (C03).

Animation key-pose sheet in two rows. Top row, the kicker from the kicker turnaround, four poses: a knee slide, an arms-wide aeroplane run, a finger to the lips, and hands on head after a miss. Bottom row, the keeper from the keeper turnaround, four poses: a roaring fist pump, pointing to the crowd, lying face down after conceding, and standing with hands on hips.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## Ball, goal and pitch

Props I model to real dimensions: a 22 cm ball, a 7.32 × 2.44 m goal, the 9.15 m spray line.

### B01 · Three era balls

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/props/B01-era-balls.png`
- **Attach, in order:** A01
- **I'll build:** The three balls, each paired with its era's physics in the game.

**ChatGPT**

```text
Make a landscape image (3:2). Orthographic reference of three original footballs in one row, each shown from the front and from the side. A 1970s ball of hand-stitched brown leather in the classic 32-panel pattern of hexagons and pentagons, slightly worn and scuffed. A 1990s glossy white ball with bold original curved-triangle graphics in black and gold. A modern lightweight ball with smooth bonded panels and an original pattern of sweeping teal and gold strokes.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Orthographic reference of three original footballs in one row, each shown from the front and from the side. A 1970s ball of hand-stitched brown leather in the classic 32-panel pattern of hexagons and pentagons, slightly worn and scuffed. A 1990s glossy white ball with bold original curved-triangle graphics in black and gold. A modern lightweight ball with smooth bonded panels and an original pattern of sweeping teal and gold strokes.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### B02 · Goal frame and net

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/props/B02-goal-net.png`
- **Attach, in order:** A01
- **I'll build:** The goal to real dimensions, with a net that ripples and bulges on a goal.

**ChatGPT**

```text
Make a landscape image (3:2). Orthographic reference of a full-size football goal, front view and side view next to each other: slim, rounded white posts and crossbar, a deep net held out by thin back supports, and nylon net mesh drawn with fine, even ink lines. Add a close-up inset of the net knots in the corner.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Orthographic reference of a full-size football goal, front view and side view next to each other: slim, rounded white posts and crossbar, a deep net held out by thin back supports, and nylon net mesh drawn with fine, even ink lines. Add a close-up inset of the net knots in the corner.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### B03 · Penalty area details

- **Batch:** later
- **Image shape:** Square 1:1
- **Save as:** `art/reference/props/B03-penalty-area.png`
- **Attach, in order:** A01
- **I'll build:** Line markings, the penalty spot, the spray line and the corner flag.

**ChatGPT**

```text
Make a square image (1:1). Overhead view of the penalty area of a real football pitch: crisp painted white lines, the penalty spot, a curved line of white vanishing spray on the grass, and, beside the frame, a tall corner flag shown from the side.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the attached image, the style keyframe (A01), as your reference.

Overhead view of the penalty area of a real football pitch: crisp painted white lines, the penalty spot, a curved line of white vanishing spray on the grass, and, beside the frame, a tall corner flag shown from the side.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### B04 · Floodlights through the eras

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/props/B04-floodlights.png`
- **Attach, in order:** A01
- **I'll build:** One floodlight design per era.

**ChatGPT**

```text
Make a landscape image (3:2). Three original stadium floodlight designs side by side, all seen from the front: a 1970s lattice steel pylon topped with a dense grid of lamps, a 1990s angular tower with rectangular lamp banks, and a modern floodlight ring set into the edge of a stadium roof.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Three original stadium floodlight designs side by side, all seen from the front: a 1970s lattice steel pylon topped with a dense grid of lamps, a 1990s angular tower with rectangular lamp banks, and a modern floodlight ring set into the edge of a stadium roof.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## Arena and cameras

The first playable ground, the harbour stadium, and the camera framings the shootout uses on a phone.

### W01 · The harbour stadium from above

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/arena/W01-stadium-aerial.png`
- **Attach, in order:** A01
- **I'll build:** The layout and scale of the first playable ground.

**ChatGPT**

```text
Make a landscape image (3:2). Aerial three-quarter view of the harbourfront stadium from the attached keyframe at golden hour: a modern bowl with a sweeping translucent roof, three closed sides and one open end looking out over a harbour with moored fishing boats, a lighthouse on the breakwater, and the sun setting over the sea. Old brick warehouses and narrow streets surround the stadium, with fans streaming in. One end of the pitch, with its goal, is in the foreground.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Aerial three-quarter view of the harbourfront stadium from the attached keyframe at golden hour: a modern bowl with a sweeping translucent roof, three closed sides and one open end looking out over a harbour with moored fishing boats, a lighthouse on the breakwater, and the sun setting over the sea. Old brick warehouses and narrow streets surround the stadium, with fans streaming in. One end of the pitch, with its goal, is in the foreground.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### W02 · Game camera: behind the ball

- **Batch:** first
- **Image shape:** Portrait 2:3
- **Save as:** `art/reference/arena/W02-camera-kicker.png`
- **Attach, in order:** A01, W01
- **I'll build:** The main kicker camera, framed for a phone.

**ChatGPT**

```text
Make a portrait image (2:3). The game camera's view from just behind and above the ball on the penalty spot, looking at the goal in the attached stadium. The keeper is set on his line, the steep stand behind the goal is packed with fans and scarves, and the evening sky glows above the roof. Framed for a phone held upright: the goal fills the upper half, and the ball sits in the lower third with clear space around it.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 2:3 portrait format. Use the reference images attached in this order: first, the style keyframe (A01); second, the stadium overview (W01).

The game camera's view from just behind and above the ball on the penalty spot, looking at the goal in the attached stadium. The keeper is set on his line, the steep stand behind the goal is packed with fans and scarves, and the evening sky glows above the roof. Framed for a phone held upright: the goal fills the upper half, and the ball sits in the lower third with clear space around it.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### W03 · Game camera: keeper's view

- **Batch:** first
- **Image shape:** Portrait 2:3
- **Save as:** `art/reference/arena/W03-camera-keeper.png`
- **Attach, in order:** A01, W01
- **I'll build:** The keeper camera, framed for a phone.

**ChatGPT**

```text
Make a portrait image (2:3). The keeper's view from just in front of the goal line at eye height, looking out at the kicker from the attached keyframe standing behind the ball 11 metres away. Behind the kicker, the open end of the stadium frames the harbour and the setting sun. Framed for a phone held upright, with the kicker small and central and the goal posts just out of frame on either side.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 2:3 portrait format. Use the reference images attached in this order: first, the style keyframe (A01); second, the stadium overview (W01).

The keeper's view from just in front of the goal line at eye height, looking out at the kicker from the attached keyframe standing behind the ball 11 metres away. Behind the kicker, the open end of the stadium frames the harbour and the setting sun. Framed for a phone held upright, with the kicker small and central and the goal posts just out of frame on either side.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### W04 · Stands and crowd up close

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/arena/W04-stands-crowd.png`
- **Attach, in order:** A01
- **I'll build:** Stand geometry and the crowd shader.

**ChatGPT**

```text
Make a landscape image (3:2). A close view of one tier of the stands: fans in scarves and jackets drawn with ink outlines and halftone shading, flags and hand-painted banners with abstract patterns, a few phone torches, and flare smoke drifting through low golden sunlight in loose inked swirls.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A close view of one tier of the stands: fans in scarves and jackets drawn with ink outlines and halftone shading, flags and hand-painted banners with abstract patterns, a few phone torches, and flare smoke drifting through low golden sunlight in loose inked swirls.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## Moments

Frames that direct the animation, lighting and effects at the beats that matter: the stare-down, the goal, the save, the replay.

### M01 · The stare-down

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M01-stare-down.png`
- **Attach, in order:** A01, C02, C04
- **I'll build:** The stare-down cinematic camera and face lighting.

**ChatGPT**

```text
Make a landscape image (3:2). Two tall comic panels side by side at the moment before a penalty: the left panel is an extreme close-up of the kicker's eyes and brow from the kicker face sheet, the right panel the keeper's eyes from the keeper face sheet. Both are lit by low golden sunlight with bold ink shadows, and the stadium behind them breaks into halftone dots and offset colour. The tension of two friends trying to read each other.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker face sheet (C02); third, the keeper face sheet (C04).

Two tall comic panels side by side at the moment before a penalty: the left panel is an extreme close-up of the kicker's eyes and brow from the kicker face sheet, the right panel the keeper's eyes from the keeper face sheet. Both are lit by low golden sunlight with bold ink shadows, and the stadium behind them breaks into halftone dots and offset colour. The tension of two friends trying to read each other.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### M02 · The goal

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M02-goal.png`
- **Attach, in order:** A01, B02
- **I'll build:** The net ripple, crowd eruption and camera for a goal.

**ChatGPT**

```text
Make a landscape image (3:2). The instant a penalty hits the back of the net in the attached stadium: the net bulges, the keeper lies on the grass where he dived, and the stand behind the goal erupts, with scarves whirling, paper confetti in the air and flare smoke catching the evening light. Graphic impact lines burst outward from the net.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the goal reference (B02).

The instant a penalty hits the back of the net in the attached stadium: the net bulges, the keeper lies on the grass where he dived, and the stand behind the goal erupts, with scarves whirling, paper confetti in the air and flare smoke catching the evening light. Graphic impact lines burst outward from the net.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### M03 · The fingertip save

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M03-fingertip-save.png`
- **Attach, in order:** A01, C03
- **I'll build:** The slow-motion save, with water spray under the floodlights.

**ChatGPT**

```text
Make a landscape image (3:2). A frozen slow-motion moment under floodlights on a wet night: the keeper from the attached turnaround at full stretch, tipping the ball around the post with his fingertips. A spray of water flies off the ball and his gloves, each droplet catching the light, and the crowd behind breaks into halftone dots and offset colour. His body shows a motion smear and a few overlapping ink outlines from the dive.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

A frozen slow-motion moment under floodlights on a wet night: the keeper from the attached turnaround at full stretch, tipping the ball around the post with his fingertips. A spray of water flies off the ball and his gloves, each droplet catching the light, and the crowd behind breaks into halftone dots and offset colour. His body shows a motion smear and a few overlapping ink outlines from the dive.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### M04 · The slow-motion replay

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M04-replay.png`
- **Attach, in order:** A01, A03
- **I'll build:** The replay camera and tracer styling.

**ChatGPT**

```text
Make a landscape image (3:2). A side-on slow-motion replay frame: the full flight of a knuckleball drawn as a bold inked trajectory with speed lines that wobbles near the goal, the ball frozen in mid-air above a jumping wall of crimson players, and the keeper beginning his dive.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the replay tracer board (A03).

A side-on slow-motion replay frame: the full flight of a knuckleball drawn as a bold inked trajectory with speed lines that wobbles near the goal, the ball frozen in mid-air above a jumping wall of crimson players, and the keeper beginning his dive.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## Textures

Surfaces I map onto the models. Most are square images.

### T01 · Stadium turf

- **Batch:** first
- **Image shape:** Square 1:1
- **Save as:** `art/reference/textures/T01-grass.png`
- **Attach, in order:** A01
- **I'll build:** The pitch surface.

**ChatGPT**

```text
Make a square image (1:1). Stadium turf drawn in a comic-book style: deep greens in flat colour with fine ink hatching for grass blades and light halftone dots, with alternating wide light and dark mowing stripes running vertically.

This is a texture for a 3D game in a comic-book style: flat colour with ink hatching and halftone dot detail, matching the attached style reference. View it straight down, with flat, even lighting and no shadows. It must tile seamlessly on all four edges and fill the whole frame edge to edge.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the attached image, the style keyframe (A01), as your reference.

Stadium turf drawn in a comic-book style: deep greens in flat colour with fine ink hatching for grass blades and light halftone dots, with alternating wide light and dark mowing stripes running vertically.

This is a texture for a 3D game in a comic-book style: flat colour with ink hatching and halftone dot detail, in the look of the style keyframe. Show the surface from directly above, evenly and flatly lit, so it reads as pure colour and pattern. Make it seamless, so the left edge flows into the right and the top into the bottom, and fill the whole frame edge to edge.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### T02 · Worn goalmouth

- **Batch:** later
- **Image shape:** Square 1:1
- **Save as:** `art/reference/textures/T02-goalmouth.png`
- **Attach, in order:** A01, T01
- **I'll build:** The scuffed turf in front of the goal.

**ChatGPT**

```text
Make a square image (1:1). Worn goalmouth turf in the same comic style as the attached grass: thinning grass, scuffed bare-earth patches and stud marks, drawn with ink hatching and halftone dots.

This is a texture for a 3D game in a comic-book style: flat colour with ink hatching and halftone dot detail, matching the attached style reference. View it straight down, with flat, even lighting and no shadows. It must tile seamlessly on all four edges and fill the whole frame edge to edge.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the reference images attached in this order: first, the style keyframe (A01); second, the grass texture (T01).

Worn goalmouth turf in the same comic style as the attached grass: thinning grass, scuffed bare-earth patches and stud marks, drawn with ink hatching and halftone dots.

This is a texture for a 3D game in a comic-book style: flat colour with ink hatching and halftone dot detail, in the look of the style keyframe. Show the surface from directly above, evenly and flatly lit, so it reads as pure colour and pattern. Make it seamless, so the left edge flows into the right and the top into the bottom, and fill the whole frame edge to edge.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### T03 · Crowd sprites

- **Batch:** first
- **Image shape:** Square 1:1
- **Save as:** `art/reference/textures/T03-crowd-sprites.png`
- **Attach, in order:** A01
- **I'll build:** Billboard sprites for thousands of fans, cut out on the green background.
- **Tip:** The pure green background is what lets me cut the fans out. If it comes back with a floor or shadows, ask again for a flat #00FF00 background.

**ChatGPT**

```text
Make a square image (1:1). A sheet of sixteen different football fans in a four-by-four grid, each a full-body figure in the comic style, with ink outlines, flat colour and light halftone shading, in a different pose: cheering with arms up, clapping, holding a scarf over the head, hands on head, jumping. Every figure is on a flat, pure green background (#00FF00) with clear space between figures. This overrides the grey background for this sheet.

This is a reference sheet for 3D modelling and animation in Blender. Comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the attached image, the style keyframe (A01), as your reference.

A sheet of sixteen different football fans in a four-by-four grid, each a full-body figure in the comic style, with ink outlines, flat colour and light halftone shading, in a different pose: cheering with arms up, clapping, holding a scarf over the head, hands on head, jumping. Every figure is on a flat, pure green background (#00FF00) with clear space between figures. This overrides the grey background for this sheet.

This is a reference sheet for 3D modelling and animation in Blender. Draw it in a comic-book 3D style: clean hand-inked contour lines, bold flat colour fills and light halftone shading, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### T04 · Sunset sky dome

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/textures/T04-sky-dome.png`
- **Attach, in order:** A01
- **I'll build:** The 360-degree sky around the harbour stadium.

**ChatGPT**

```text
Make a landscape image (3:2). A wide panoramic sunset sky over the sea for a 360-degree sky dome, drawn like a comic: a warm gold sun low on the horizon, bands of flat colour from orange and rose to clear blue overhead filled with Ben-Day dots, and clouds with crisp inked edges. The horizon runs exactly across the middle of the image, with no ground, buildings or people.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A wide panoramic sunset sky over the sea for a 360-degree sky dome, drawn like a comic: a warm gold sun low on the horizon, bands of flat colour from orange and rose to clear blue overhead filled with Ben-Day dots, and clouds with crisp inked edges. The horizon runs exactly across the middle of the image, and the whole frame is sky, cloud and a thin line of calm sea.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## Eras and Classics

The other grounds: the two remaining eras and the Classics scenario cards. These come after the first batch.

### E01 · 1970s: the old ground on a winter afternoon

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/E01-era-1970s.png`
- **Attach, in order:** A01
- **I'll build:** The 1970s era, paired with the heavy leather ball.

**ChatGPT**

```text
Make a landscape image (3:2). The penalty end of an original old football ground on a winter afternoon in the 1970s: deep standing terraces packed with fans in heavy coats and scarves, a low sun behind the main stand, breath clouding in the cold air, a heavy muddy goalmouth, a lattice floodlight pylon, and a brown leather ball on the spot. Drawn like a 1970s newsprint comic: limited four-colour printing, coarse halftone dots, slightly off-register colour and a yellowed paper tone.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

The penalty end of an original old football ground on a winter afternoon in the 1970s: deep standing terraces packed with fans in heavy coats and scarves, a low sun behind the main stand, breath clouding in the cold air, a heavy muddy goalmouth, a lattice floodlight pylon, and a brown leather ball on the spot. Drawn like a 1970s newsprint comic: limited four-colour printing, coarse halftone dots, slightly off-register colour and a yellowed paper tone.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### E02 · 1990s: floodlit rain

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/E02-era-1990s.png`
- **Attach, in order:** A01
- **I'll build:** The 1990s era, paired with the glossy, big-bending ball.

**ChatGPT**

```text
Make a landscape image (3:2). The penalty end of an original city ground on a floodlit night in the 1990s: rain falling through the floodlight beams, steam rising off the packed crowd, blank advertising boards along the touchline, a glossy white ball on the wet spot, and puddles reflecting the lights. Drawn like a 1990s comic: heavy black inks, saturated colour, airbrushed gradients and glossy highlights.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

The penalty end of an original city ground on a floodlit night in the 1990s: rain falling through the floodlight beams, steam rising off the packed crowd, blank advertising boards along the touchline, a glossy white ball on the wet spot, and puddles reflecting the lights. Drawn like a 1990s comic: heavy black inks, saturated colour, airbrushed gradients and glossy highlights.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### S01 · Classic: The Impossible Banana

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S01-classic-impossible-banana.png`
- **Attach, in order:** A01, C05
- **I'll build:** The 35 m free kick that has to start wide of the post.

**ChatGPT**

```text
Make a landscape image (3:2). A free kick 35 metres from goal at an original small coastal ground on a clifftop at golden hour, with the sea far below beyond the low stand. A wall of crimson players stands at the spray line, and a bold inked trajectory with speed lines starts far wide of the post before bending back toward the far corner.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the wall players sheet (C05).

A free kick 35 metres from goal at an original small coastal ground on a clifftop at golden hour, with the sea far below beyond the low stand. A wall of crimson players stands at the spray line, and a bold inked trajectory with speed lines starts far wide of the post before bending back toward the far corner.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### S02 · Classic: Ninety-Third Minute

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S02-classic-ninety-third-minute.png`
- **Attach, in order:** A01, C05
- **I'll build:** One kick, a wall of six, everything on it.

**ChatGPT**

```text
Make a landscape image (3:2). A storm at night at a packed old ground: a wall of six crimson players braced at the spray line, rain lashing sideways through the floodlight beams, lightning over the stand, and the kicker alone behind the ball.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the wall players sheet (C05).

A storm at night at a packed old ground: a wall of six crimson players braced at the spray line, rain lashing sideways through the floodlight beams, lightning over the stand, and the kicker alone behind the ball.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### S03 · Classic: The Ice-Cold Chip

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S03-classic-ice-cold-chip.png`
- **Attach, in order:** A01, C03
- **I'll build:** The keeper who always dives early.

**ChatGPT**

```text
Make a landscape image (3:2). A snowy evening at a northern ground: snow falling through the floodlights, a bright orange winter ball, the pitch lines brushed clear, and the keeper diving early into the snow while a soft chip floats over him.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

A snowy evening at a northern ground: snow falling through the floodlights, a bright orange winter ball, the pitch lines brushed clear, and the keeper diving early into the snow while a soft chip floats over him.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### S04 · Classic: Monsoon Final

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S04-classic-monsoon-final.png`
- **Attach, in order:** A01
- **I'll build:** The wet, skidding ball.

**ChatGPT**

```text
Make a landscape image (3:2). A cup final in a monsoon at a tropical ground at dusk: warm torrential rain, palm trees beyond the low stand, kits soaked through, and the ball skidding across the waterlogged grass, throwing up a spray of water.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A cup final in a monsoon at a tropical ground at dusk: warm torrential rain, palm trees beyond the low stand, kits soaked through, and the ball skidding across the waterlogged grass, throwing up a spray of water.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

### S05 · Classic: Thin Air

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S05-classic-thin-air.png`
- **Attach, in order:** A01
- **I'll build:** The high-altitude stadium where the ball flies further.

**ChatGPT**

```text
Make a landscape image (3:2). An original stadium high in the mountains on a clear afternoon: thin, bright air, sharp sunlight and crisp shadows, snow-capped peaks rising behind the stands, and clouds drifting in the valley below.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Real stadium architecture, turf, crowds and kits, drawn with hand-inked contour lines over 3D forms, bold flat colour, halftone and Ben-Day dot shading in shadows and skies, and offset-print colour misregistration where things fall out of focus, instead of blur. Graphic speed lines and motion smears on fast action. Dramatic light: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. The goal frame, the ball and the players have the cleanest lines and the strongest contrast. Dynamic, comic-panel composition with bold camera angles.

Original design: not based on any real footballer, club, national team, stadium or brand, or on any existing film, comic or video game. No text, letters, numbers, logos, crests, sponsor marks, sound-effect lettering or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

An original stadium high in the mountains on a clear afternoon: thin, bright air, sharp sunlight and crisp shadows, snow-capped peaks rising behind the stands, and clouds drifting in the valley below.

Art direction: Free Kick Classics, an original football game set in believable football grounds, rendered as a comic book come to life in 3D. Draw real stadium architecture, turf, crowds and kits with hand-inked contour lines over 3D forms and bold flat colour. Shade shadows and skies with halftone and Ben-Day dots, and show anything out of focus as offset-print colour misregistration, with every edge still crisp. Add graphic speed lines and motion smears to fast action. Light it dramatically: golden hours, floodlights cutting through haze and rain, long shadows, drifting flare smoke. Give the goal frame, the ball and the players the cleanest lines and the strongest contrast, and compose like a dynamic comic panel with bold camera angles.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new, and none of it comes from an existing film, comic or video game. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Impact moments use graphic bursts and lines rather than lettering. Keep the whole image free of text, numbers, logos and watermarks.
```

## What I'll build in Blender

**Scenes**

- **The comic render setup.** One reusable Blender look for every scene: Line Art ink lines, halftone and Ben-Day dot shading, offset-colour defocus instead of blur, and a print style per era.
- **The harbour stadium.** The first playable ground, at golden hour and under floodlights: its stands, crowd, sky, sea and lighting, with the phone cameras for kicker, keeper and the split-panel stare-down.
- **The free-kick set piece.** The same ground with a wall at the 9.15 m spray line and the referee.
- **Two more eras.** The 1970s old ground on a winter afternoon and the 1990s floodlit rain, each lit and graded as its own ground.
- **Classics scenes.** Clifftop coast, night storm, snow, monsoon and mountain altitude, one per scenario card.

**Animations**

- **Kicker.** Ready idle, run-up, stutter, side-foot, driven knuckleball, curler, trivela, chip, two celebrations, a miss. Animated on twos, with smear frames on the strikes.
- **Keeper.** Ready stance, shuffle, low, middle and top-corner dives to both sides, staying big, catch, smother, fingertip save, punch, celebration, despair.
- **Wall.** Line up, jump, brace, the lying man.
- **Ball and effects.** Inked replay trajectories with speed lines that follow flight paths exported from the game's physics, impact bursts on the net, scarves and flags, rain, snow and flare smoke.

You get `.blend` scenes, FBX characters and animations ready for Unity, and preview renders and videos along the way.

## Sending things back

- **Images:** save each under the name on its prompt (alternatives get `-v2`), then attach them in our chat or upload them to their folder under `art/reference/`.
- **Models:** any format Blender opens: `.blend`, `.fbx`, `.glb`/`.gltf` or `.obj`. Metres as units is ideal. A humanoid rig is a bonus; I can rig unrigged characters.
- **Rights:** only send models and images you made or are allowed to use in a commercial game, and tell me any credit a licence requires.
