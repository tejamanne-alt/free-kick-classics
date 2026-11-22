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
Make a landscape image (3:2). Cinematic wide frame of a penalty kick at night in a dreamlike arena. In the foreground a kicker in a deep teal kit stands behind the ball on the penalty spot, seen from behind and slightly to one side. On the goal line the keeper, in a warm amber kit, crouches with his eyes locked on the kicker. The white goal frame glows faintly. Around them the stadium rises into towering tiers of softly glowing crowd silhouettes that blur upward into an indigo sky full of green and violet aurora. A thin ribbon of gold light curls from the ball toward the top corner of the goal, as if showing the shot the kicker is imagining. Low mist drifts across grass painted with rich mowing stripes. The mood is a held breath just before the strike.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format.

Cinematic wide frame of a penalty kick at night in a dreamlike arena. In the foreground a kicker in a deep teal kit stands behind the ball on the penalty spot, seen from behind and slightly to one side. On the goal line the keeper, in a warm amber kit, crouches with his eyes locked on the kicker. The white goal frame glows faintly. Around them the stadium rises into towering tiers of softly glowing crowd silhouettes that blur upward into an indigo sky full of green and violet aurora. A thin ribbon of gold light curls from the ball toward the top corner of the goal, as if showing the shot the kicker is imagining. Low mist drifts across grass painted with rich mowing stripes. The mood is a held breath just before the strike.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### A02 · Palette and materials board

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/style/A02-palette-materials.png`
- **Attach, in order:** A01
- **I'll build:** The Blender material library and the colour values every scene shares.

**ChatGPT**

```text
Make a landscape image (3:2). A tidy art-direction board on a dark indigo background, using the colours of the attached keyframe. Across the top, a row of eight plain colour swatches: night indigo, violet, teal, aurora green, warm gold, amber, chalk white and deep pitch green. Below them, six glossy material spheres in a row showing how surfaces should look: painterly striped grass, stitched ball leather, smooth modern ball synthetic, the soft glow of the crowd, goal-net mesh with a faint glow on its knots, and kit fabric with a subtle woven texture.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A tidy art-direction board on a dark indigo background, using the colours of the attached keyframe. Across the top, a row of eight plain colour swatches: night indigo, violet, teal, aurora green, warm gold, amber, chalk white and deep pitch green. Below them, six glossy material spheres in a row showing how surfaces should look: painterly striped grass, stitched ball leather, smooth modern ball synthetic, the soft glow of the crowd, goal-net mesh with a faint glow on its knots, and kit fabric with a subtle woven texture.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### A03 · Light language: four ball flights

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/style/A03-ball-flight-trails.png`
- **Attach, in order:** A01
- **I'll build:** The ball-trail effects. In Blender the ribbons follow real flight paths exported from the game's own physics.

**ChatGPT**

```text
Make a landscape image (3:2). Four panels side by side, each showing a football's flight as a ribbon of light against a dark painterly sky, seen from the side. Panel one: a smooth bending curve for a curling shot, in gold. Panel two: an arc that dips sharply at the end for a topspin shot, in violet. Panel three: a nearly straight path that wobbles and shimmers only in its last third, for a knuckleball, in cyan. Panel four: a high, gentle lob for a chip, in soft white. In every panel the ribbon is brightest at the ball and fades into sparkling dust behind it.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Four panels side by side, each showing a football's flight as a ribbon of light against a dark painterly sky, seen from the side. Panel one: a smooth bending curve for a curling shot, in gold. Panel two: an arc that dips sharply at the end for a topspin shot, in violet. Panel three: a nearly straight path that wobbles and shimmers only in its last third, for a knuckleball, in cyan. Panel four: a high, gentle lob for a chip, in soft white. In every panel the ribbon is brightest at the ball and fades into sparkling dust behind it.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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
Make a landscape image (3:2). Character turnaround model sheet of the kicker from the attached keyframe: an original young footballer with an athletic, slightly elongated stylized build, short curly dark hair, warm brown skin and determined eyes. Deep teal kit with a subtle gold wave pattern across the shirt, white shorts, teal socks and gold boots. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Character turnaround model sheet of the kicker from the attached keyframe: an original young footballer with an athletic, slightly elongated stylized build, short curly dark hair, warm brown skin and determined eyes. Deep teal kit with a subtle gold wave pattern across the shirt, white shorts, teal socks and gold boots. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Head and face reference sheet for the kicker in the attached turnaround. Top row: the neutral head from the front, three-quarter and side. Bottom row: four expressions from the front: calm focus, a sly bluffing half-smile, wide-eyed joy after scoring, and frustration after a miss. Shoulders up, with the same face shape and hair in every view.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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
Make a landscape image (3:2). Character turnaround model sheet of the keeper from the attached keyframe: an original goalkeeper, tall with long arms and large hands, stylized proportions, shaved head with a short beard, light tan skin and calm, intense eyes. Long-sleeved warm amber goalkeeper kit with darker amber elbow pads and a faint geometric pattern, black shorts, amber socks, and chunky gloves whose fingertips glow softly. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Character turnaround model sheet of the keeper from the attached keyframe: an original goalkeeper, tall with long arms and large hands, stylized proportions, shaved head with a short beard, light tan skin and calm, intense eyes. Long-sleeved warm amber goalkeeper kit with darker amber elbow pads and a faint geometric pattern, black shorts, amber socks, and chunky gloves whose fingertips glow softly. Three full-body views side by side: front, left side and back. A-pose, with arms angled 45 degrees down and feet shoulder-width apart.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Face and glove reference sheet for the keeper in the attached turnaround. Top row: the neutral head from the front, three-quarter and side. Bottom row: four expressions from the front: reading the kicker with narrowed eyes, a taunting grin, a roaring celebration, and quiet despair. On the right, a close study of one goalkeeper glove from the palm side and from the back.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Turnaround sheet of three original outfield players who will form a defensive wall, all in the same deep crimson kit with a faint diagonal pattern: one broad and short, one tall and lean, one of medium build, each with a different hairstyle and skin tone. Show each player from the front and from the left side, standing in an A-pose, so there are six full-body views in one row.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01); third, the keeper turnaround (C03).

Flat-lay kit design sheet seen straight down: four complete football kits, each laid out neatly with shirt, shorts and socks and shown from the front and the back. The kicker's teal and gold kit from the kicker turnaround, an alternate kit in chalk white and violet, the keeper's amber kit from the keeper turnaround, and the wall team's deep crimson kit. Fabric weave and pattern detail clearly visible.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### C07 · The referee

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/characters/C07-referee.png`
- **Attach, in order:** A01
- **I'll build:** The referee who sprays the 9.15 m line and pushes the wall back.

**ChatGPT**

```text
Make a landscape image (3:2). Character turnaround model sheet of an original referee with a calm, theatrical presence, like a conductor: night-black kit with luminous gold piping, a whistle on a cord, and a can of vanishing spray in one hand. Three full-body views side by side: front, left side and back, in an A-pose.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Character turnaround model sheet of an original referee with a calm, theatrical presence, like a conductor: night-black kit with luminous gold piping, a whistle on a cord, and a can of vanishing spray in one hand. Three full-body views side by side: front, left side and back, in an A-pose.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround taking a penalty, seen from the side. Eight poses in a single row from left to right, evenly spaced: standing ready behind the ball; first step; mid-stride; long last stride with the planted foot landing beside the ball; kicking leg swinging back; side-foot contact with the ball; follow-through; landing and looking up.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround striking a driven knuckleball, seen from the side. Eight poses in a single row from left to right, evenly spaced: ready; fast approach; planted foot; kicking leg cocked high; laces striking through the middle of the ball with the ankle locked; short, stabbing follow-through; landing; watching the ball fly.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround, seen from the side, in two rows. Top row, four poses of an inside-of-the-foot curler, with the body wrapping around the ball. Bottom row, four poses of an outside-of-the-foot trivela, with the kicking leg swinging across the body.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround playing a soft chip down the middle, seen from the side. Six poses in a single row from left to right: ready; approach, slowing; planted foot; kicking leg swinging gently; foot scooping under the ball; a cheeky follow-through with the weight leaning back.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01).

Animation key-pose sheet of the kicker in the attached turnaround doing a stutter run-up, seen from the side. Six poses in a single row from left to right: approach; a sudden hop and pause mid-run with the arms out for balance; a second small hop; the long final stride; the strike; follow-through.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Animation key-pose sheet of the keeper in the attached turnaround, seen from the front. Six poses in a single row from left to right: crouched ready stance on the goal line; shuffle step to his left; shuffle step to his right; arms spread wide to look big; bouncing on his toes; set position a moment before the dive.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Animation key-pose sheet of the keeper in the attached turnaround, seen from the front as the kicker sees him, in three rows of four poses, all diving to the same side. Top row, a low dive to the bottom corner. Middle row, a dive at waist height. Bottom row, a full-stretch leap to the top corner. Each row runs from left to right: push-off, take-off, full stretch at the height of the ball, landing.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

Animation key-pose sheet of the keeper in the attached turnaround, seen from the front. Six poses in a single row from left to right: staying big in a star shape with arms and legs spread; catching a ball against his chest; smothering a low ball on the ground; a one-handed fingertip tip over the bar; a two-fisted punch; getting back up from the ground.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the wall players sheet (C05).

Animation key-pose sheet of the wall players in the attached sheet, seen from the side. Eight poses in a single row from left to right: lining up shoulder to shoulder; hands covering in the set position; crouching before the jump; jumping together with arms down; landing; one player staying grounded to block a low shot; one player lying flat on the ground behind the wall; turning to watch the ball.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker turnaround (C01); third, the keeper turnaround (C03).

Animation key-pose sheet in two rows. Top row, the kicker from the kicker turnaround, four poses: a knee slide, an arms-wide aeroplane run, a finger to the lips, and hands on head after a miss. Bottom row, the keeper from the keeper turnaround, four poses: a roaring fist pump, pointing to the crowd, lying face down after conceding, and standing with hands on hips.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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
Make a landscape image (3:2). Orthographic reference of three original footballs in one row, each shown from the front and from the side. A 1970s ball of hand-stitched brown leather in the classic 32-panel pattern of hexagons and pentagons, slightly worn and scuffed. A 1990s glossy white ball with bold original curved-triangle graphics in black and gold. A modern lightweight ball with smooth bonded panels and an original iridescent teal and violet swirl pattern.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Orthographic reference of three original footballs in one row, each shown from the front and from the side. A 1970s ball of hand-stitched brown leather in the classic 32-panel pattern of hexagons and pentagons, slightly worn and scuffed. A 1990s glossy white ball with bold original curved-triangle graphics in black and gold. A modern lightweight ball with smooth bonded panels and an original iridescent teal and violet swirl pattern.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### B02 · Goal frame and net

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/props/B02-goal-net.png`
- **Attach, in order:** A01
- **I'll build:** The goal to real dimensions, with a net that ripples and glows on a goal.

**ChatGPT**

```text
Make a landscape image (3:2). Orthographic reference of a full-size football goal in the attached style, front view and side view next to each other: slim, rounded white posts and crossbar, a deep net held out by thin back supports, and net mesh that glows faintly where light catches the knots. Add a close-up inset of the net knots in the corner.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Orthographic reference of a full-size football goal in the attached style, front view and side view next to each other: slim, rounded white posts and crossbar, a deep net held out by thin back supports, and net mesh that glows faintly where light catches the knots. Add a close-up inset of the net knots in the corner.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### B03 · Penalty area details

- **Batch:** later
- **Image shape:** Square 1:1
- **Save as:** `art/reference/props/B03-penalty-area.png`
- **Attach, in order:** A01
- **I'll build:** Line markings, the penalty spot, the spray line and the corner flag.

**ChatGPT**

```text
Make a square image (1:1). Overhead view of the penalty area of a football pitch in the attached style: crisp painted white lines, the penalty spot, a curved line of white vanishing spray on the grass, and, beside the frame, a tall corner flag shown from the side.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the attached image, the style keyframe (A01), as your reference.

Overhead view of the penalty area of a football pitch in the attached style: crisp painted white lines, the penalty spot, a curved line of white vanishing spray on the grass, and, beside the frame, a tall corner flag shown from the side.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### B04 · Floodlights through the eras

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/props/B04-floodlights.png`
- **Attach, in order:** A01
- **I'll build:** One floodlight design per era.

**ChatGPT**

```text
Make a landscape image (3:2). Three original floodlight designs side by side, all seen from the front: a 1970s lattice steel tower topped with round lamps, a 1990s angular chrome tower with rectangular lamp banks, and a modern slender arch that is itself a line of light.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Three original floodlight designs side by side, all seen from the front: a 1970s lattice steel tower topped with round lamps, a 1990s angular chrome tower with rectangular lamp banks, and a modern slender arch that is itself a line of light.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

## Arena and cameras

The first playable world and the camera framings the shootout uses on a phone.

### W01 · The dream arena from above

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/arena/W01-arena-aerial.png`
- **Attach, in order:** A01
- **I'll build:** The layout and scale of the first playable arena.

**ChatGPT**

```text
Make a landscape image (3:2). Aerial three-quarter view of the dream arena from the attached keyframe at night. The pitch floats on a still, dark lake that mirrors the sky, ringed by towering, gently impossible curved stands whose crowd glows like a sea of tiny warm lights. Slender arches of light stand where floodlights would be. The aurora above washes everything in green and violet. One end of the pitch, with its goal, is in the foreground.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Aerial three-quarter view of the dream arena from the attached keyframe at night. The pitch floats on a still, dark lake that mirrors the sky, ringed by towering, gently impossible curved stands whose crowd glows like a sea of tiny warm lights. Slender arches of light stand where floodlights would be. The aurora above washes everything in green and violet. One end of the pitch, with its goal, is in the foreground.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### W02 · Game camera: behind the ball

- **Batch:** first
- **Image shape:** Portrait 2:3
- **Save as:** `art/reference/arena/W02-camera-kicker.png`
- **Attach, in order:** A01, W01
- **I'll build:** The main kicker camera, framed for a phone.

**ChatGPT**

```text
Make a portrait image (2:3). The game camera's view from just behind and above the ball on the penalty spot, looking at the goal in the attached arena. The keeper is set on his line, and the glowing stands rise behind the goal into the aurora sky. Framed for a phone held upright: the goal fills the upper half, and the ball sits in the lower third with clear space around it.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 2:3 portrait format. Use the reference images attached in this order: first, the style keyframe (A01); second, the arena overview (W01).

The game camera's view from just behind and above the ball on the penalty spot, looking at the goal in the attached arena. The keeper is set on his line, and the glowing stands rise behind the goal into the aurora sky. Framed for a phone held upright: the goal fills the upper half, and the ball sits in the lower third with clear space around it.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### W03 · Game camera: keeper's view

- **Batch:** first
- **Image shape:** Portrait 2:3
- **Save as:** `art/reference/arena/W03-camera-keeper.png`
- **Attach, in order:** A01, W01
- **I'll build:** The keeper camera, framed for a phone.

**ChatGPT**

```text
Make a portrait image (2:3). The keeper's view from just in front of the goal line at eye height, looking out at the kicker from the attached keyframe standing behind the ball 11 metres away. The glowing crowd and the aurora sky fill the background. Framed for a phone held upright, with the kicker small and central and the goal posts just out of frame on either side.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 2:3 portrait format. Use the reference images attached in this order: first, the style keyframe (A01); second, the arena overview (W01).

The keeper's view from just in front of the goal line at eye height, looking out at the kicker from the attached keyframe standing behind the ball 11 metres away. The glowing crowd and the aurora sky fill the background. Framed for a phone held upright, with the kicker small and central and the goal posts just out of frame on either side.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### W04 · Stands and crowd up close

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/arena/W04-stands-crowd.png`
- **Attach, in order:** A01
- **I'll build:** Stand geometry and the crowd shader.

**ChatGPT**

```text
Make a landscape image (3:2). A close view of one tier of the arena stands in the attached style: the crowd painted as soft silhouettes holding glowing scarves, with tiny sparkles of light scattered through them and a wave of light rippling across the tier. Painterly blur toward the top of the frame.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A close view of one tier of the arena stands in the attached style: the crowd painted as soft silhouettes holding glowing scarves, with tiny sparkles of light scattered through them and a wave of light rippling across the tier. Painterly blur toward the top of the frame.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

## Moments

Frames that direct the animation, lighting and effects at the beats that matter: the stare-down, the goal, the save.

### M01 · The stare-down

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M01-stare-down.png`
- **Attach, in order:** A01, C02, C04
- **I'll build:** The stare-down cinematic camera and face lighting.

**ChatGPT**

```text
Make a landscape image (3:2). A split composition at the moment before a penalty: the left half is an extreme close-up of the kicker's eyes and brow from the kicker face sheet, the right half the keeper's eyes from the keeper face sheet. Both are rim-lit in gold, and the background melts into aurora colours. The tension of two friends trying to read each other.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the kicker face sheet (C02); third, the keeper face sheet (C04).

A split composition at the moment before a penalty: the left half is an extreme close-up of the kicker's eyes and brow from the kicker face sheet, the right half the keeper's eyes from the keeper face sheet. Both are rim-lit in gold, and the background melts into aurora colours. The tension of two friends trying to read each other.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### M02 · The goal

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M02-goal.png`
- **Attach, in order:** A01, B02
- **I'll build:** The net burst, crowd wave and camera for a goal.

**ChatGPT**

```text
Make a landscape image (3:2). The instant a penalty hits the back of the net in the attached arena: the net bulges and bursts into a shower of gold light particles, the keeper lies on the grass where he dived, and a wave of light sweeps through the crowd behind the goal.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the goal reference (B02).

The instant a penalty hits the back of the net in the attached arena: the net bulges and bursts into a shower of gold light particles, the keeper lies on the grass where he dived, and a wave of light sweeps through the crowd behind the goal.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### M03 · The fingertip save

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M03-fingertip-save.png`
- **Attach, in order:** A01, C03
- **I'll build:** The slow-motion save with afterimages.

**ChatGPT**

```text
Make a landscape image (3:2). A frozen slow-motion moment of the keeper from the attached turnaround at full stretch, tipping the ball around the post with his fingertips. Faint afterimages trail behind his body, and droplets and particles of light hang suspended in the air.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

A frozen slow-motion moment of the keeper from the attached turnaround at full stretch, tipping the ball around the post with his fingertips. Faint afterimages trail behind his body, and droplets and particles of light hang suspended in the air.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### M04 · The slow-motion replay

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/moments/M04-replay.png`
- **Attach, in order:** A01, A03
- **I'll build:** The replay camera and trail styling.

**ChatGPT**

```text
Make a landscape image (3:2). A side-on slow-motion replay frame: the full flight of a knuckleball drawn as a cyan ribbon of light that wobbles near the goal, the ball frozen in mid-air above a jumping wall of crimson players, and the keeper beginning his dive.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the light-trail board (A03).

A side-on slow-motion replay frame: the full flight of a knuckleball drawn as a cyan ribbon of light that wobbles near the goal, the ball frozen in mid-air above a jumping wall of crimson players, and the keeper beginning his dive.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

## Textures

Surfaces I map onto the models. Most are square images.

### T01 · Painterly grass

- **Batch:** first
- **Image shape:** Square 1:1
- **Save as:** `art/reference/textures/T01-grass.png`
- **Attach, in order:** A01
- **I'll build:** The pitch surface.

**ChatGPT**

```text
Make a square image (1:1). Hand-painted stylized grass, rich deep green with soft brush strokes and slight colour variation, with alternating wide light and dark mowing stripes running vertically.

This is a texture for a 3D game. View it straight down, with perfectly flat, even lighting and no shadows or highlights. It must tile seamlessly on all four edges and fill the whole frame edge to edge. Hand-painted, stylized look that matches the attached style reference.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the attached image, the style keyframe (A01), as your reference.

Hand-painted stylized grass, rich deep green with soft brush strokes and slight colour variation, with alternating wide light and dark mowing stripes running vertically.

This is a texture for a 3D game. Show the surface from directly above, evenly and flatly lit, so it reads as pure colour and pattern. Make it seamless, so the left edge flows into the right and the top into the bottom, and fill the whole frame edge to edge. Hand-painted and stylized, in the look of the style keyframe.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### T02 · Worn goalmouth

- **Batch:** later
- **Image shape:** Square 1:1
- **Save as:** `art/reference/textures/T02-goalmouth.png`
- **Attach, in order:** A01, T01
- **I'll build:** The scuffed turf in front of the goal.

**ChatGPT**

```text
Make a square image (1:1). Worn goalmouth turf in the same painted style as the attached grass: patchy grass with scuffed bare-earth patches and stud marks.

This is a texture for a 3D game. View it straight down, with perfectly flat, even lighting and no shadows or highlights. It must tile seamlessly on all four edges and fill the whole frame edge to edge. Hand-painted, stylized look that matches the attached style reference.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the reference images attached in this order: first, the style keyframe (A01); second, the grass texture (T01).

Worn goalmouth turf in the same painted style as the attached grass: patchy grass with scuffed bare-earth patches and stud marks.

This is a texture for a 3D game. Show the surface from directly above, evenly and flatly lit, so it reads as pure colour and pattern. Make it seamless, so the left edge flows into the right and the top into the bottom, and fill the whole frame edge to edge. Hand-painted and stylized, in the look of the style keyframe.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
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
Make a square image (1:1). A sheet of sixteen different stylized fans in a four-by-four grid, each a full-body painterly figure in a different pose: cheering with arms up, clapping, holding a scarf over the head, hands on head, jumping. Every figure is on a flat, pure green background (#00FF00) with clear space between figures. This overrides the grey background for this sheet.

This is a reference sheet for 3D modelling and animation in Blender. Stylized 3D look with hand-painted textures, matching the attached style reference. Orthographic views with no perspective distortion: every view at the same scale and standing on one shared ground line. Even, flat studio lighting with no cast shadows. Plain light grey background (#D8D8D8) with nothing else in frame. Do not draw numbers, arrows or labels.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a square 1:1 format. Use the attached image, the style keyframe (A01), as your reference.

A sheet of sixteen different stylized fans in a four-by-four grid, each a full-body painterly figure in a different pose: cheering with arms up, clapping, holding a scarf over the head, hands on head, jumping. Every figure is on a flat, pure green background (#00FF00) with clear space between figures. This overrides the grey background for this sheet.

This is a reference sheet for 3D modelling and animation in Blender. Render it as stylized 3D with hand-painted textures, in the look of the style keyframe. Draw every view flat and orthographic, like a technical drawing, at the same scale and standing on one shared ground line. Light it evenly from the front, as in a softbox studio, so every form reads clearly. Use a plain light grey background (#D8D8D8). The sheet holds only the drawings themselves.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### T04 · Night sky dome

- **Batch:** first
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/textures/T04-sky-dome.png`
- **Attach, in order:** A01
- **I'll build:** The 360-degree sky around the arena.

**ChatGPT**

```text
Make a landscape image (3:2). A wide panoramic night sky for a 360-degree sky dome: deep indigo with sweeping green and violet aurora curtains, scattered stars and soft painterly clouds near the horizon. The horizon runs exactly across the middle of the image, with no ground, buildings or people.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A wide panoramic night sky for a 360-degree sky dome: deep indigo with sweeping green and violet aurora curtains, scattered stars and soft painterly clouds near the horizon. The horizon runs exactly across the middle of the image, and the whole frame is sky and distant cloud.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

## Eras and Classics

The other worlds: the two remaining eras and the Classics scenario cards. These come after the first batch.

### E01 · 1970s world: the sun-bleached afternoon

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/E01-era-1970s.png`
- **Attach, in order:** A01
- **I'll build:** The 1970s era, paired with the heavy leather ball.

**ChatGPT**

```text
Make a landscape image (3:2). The same penalty end reimagined as a 1970s afternoon memory: concrete terraces packed with swaying fans in muted coats, a hazy amber sun low behind the stand, chalky pastel colours, a lattice floodlight tower, faint film grain and soft light leaks at the edges, a brown leather ball on the spot, and a worn, muddy goalmouth.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

The same penalty end reimagined as a 1970s afternoon memory: concrete terraces packed with swaying fans in muted coats, a hazy amber sun low behind the stand, chalky pastel colours, a lattice floodlight tower, faint film grain and soft light leaks at the edges, a brown leather ball on the spot, and a worn, muddy goalmouth.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### E02 · 1990s world: floodlight neon

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/E02-era-1990s.png`
- **Attach, in order:** A01
- **I'll build:** The 1990s era, paired with the glossy, big-bending ball.

**ChatGPT**

```text
Make a landscape image (3:2). The same penalty end reimagined as a 1990s night under blazing floodlights: chrome and concrete stands, magenta and teal light flares, a glossy white ball on the spot, wet grass mirroring the lights, and a faint scanline shimmer in the air.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

The same penalty end reimagined as a 1990s night under blazing floodlights: chrome and concrete stands, magenta and teal light flares, a glossy white ball on the spot, wet grass mirroring the lights, and a faint scanline shimmer in the air.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### S01 · Classic: The Impossible Banana

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S01-classic-impossible-banana.png`
- **Attach, in order:** A01, C05
- **I'll build:** The 35 m free kick that has to start wide of the post.

**ChatGPT**

```text
Make a landscape image (3:2). A free kick 35 metres from goal on a clifftop pitch at golden hour, with the sea far below. A wall of crimson players stands at the spray line, and a ribbon of gold light starts far wide of the post before bending back toward the far corner.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the wall players sheet (C05).

A free kick 35 metres from goal on a clifftop pitch at golden hour, with the sea far below. A wall of crimson players stands at the spray line, and a ribbon of gold light starts far wide of the post before bending back toward the far corner.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### S02 · Classic: Ninety-Third Minute

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S02-classic-ninety-third-minute.png`
- **Attach, in order:** A01, C05
- **I'll build:** One kick, a wall of six, everything on it.

**ChatGPT**

```text
Make a landscape image (3:2). A storm at night: a wall of six crimson players braced at the spray line, rain lashing sideways through beams of light, lightning over the stand, and the kicker alone behind the ball.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the wall players sheet (C05).

A storm at night: a wall of six crimson players braced at the spray line, rain lashing sideways through beams of light, lightning over the stand, and the kicker alone behind the ball.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### S03 · Classic: The Ice-Cold Chip

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S03-classic-ice-cold-chip.png`
- **Attach, in order:** A01, C03
- **I'll build:** The keeper who always dives early.

**ChatGPT**

```text
Make a landscape image (3:2). A frozen lake arena under a pale blue dawn, breath clouding in the cold air. The keeper has dived early into the snow while a soft chip floats over him, trailing a ribbon of white light.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the reference images attached in this order: first, the style keyframe (A01); second, the keeper turnaround (C03).

A frozen lake arena under a pale blue dawn, breath clouding in the cold air. The keeper has dived early into the snow while a soft chip floats over him, trailing a ribbon of white light.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### S04 · Classic: Monsoon Final

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S04-classic-monsoon-final.png`
- **Attach, in order:** A01
- **I'll build:** The wet, skidding ball.

**ChatGPT**

```text
Make a landscape image (3:2). Torrential warm rain at dusk with glowing raindrops, puddles mirroring neon light, and the ball skidding across the soaked grass, throwing up a spray of water.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

Torrential warm rain at dusk with glowing raindrops, puddles mirroring neon light, and the ball skidding across the soaked grass, throwing up a spray of water.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

### S05 · Classic: Above the Clouds

- **Batch:** later
- **Image shape:** Landscape 3:2
- **Save as:** `art/reference/worlds/S05-classic-above-the-clouds.png`
- **Attach, in order:** A01
- **I'll build:** The high-altitude stadium where the ball flies further.

**ChatGPT**

```text
Make a landscape image (3:2). A pitch on a mountain plateau high above the clouds: thin, bright air, sharp sunlight, clouds drifting below the edges of the stands, and the kicker's shadow stretched long across the grass.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Stylized 3D with hand-painted textures, like a frame from a painterly animated film. Immersive, escapist and slightly surreal: the world around the pitch can bend, glow and dissolve, but the football always reads clearly, because the goal frame, the ball and the players carry the strongest contrast and the cleanest shapes. Soft volumetric light, low atmospheric haze, visible brushwork in skies and crowds, luminous accents. Graceful, slightly elongated character proportions with expressive faces. Cinematic composition with depth of field.

Original design: not based on any real footballer, club, national team, stadium, brand or video game. No text, letters, numbers, logos, crests, sponsor marks or watermarks anywhere in the image.
```

**Gemini**

```text
Create an image in a 3:2 landscape format. Use the attached image, the style keyframe (A01), as your reference.

A pitch on a mountain plateau high above the clouds: thin, bright air, sharp sunlight, clouds drifting below the edges of the stands, and the kicker's shadow stretched long across the grass.

Art direction: Free Kick Classics, an original football game about a single dead-ball moment stretched into a dream. Render it as stylized 3D with hand-painted textures, like a frame from a painterly animated film. The mood is immersive, escapist and gently surreal: the world around the pitch bends, glows and dissolves into light, while the goal frame, the ball and the players stay sharp, high-contrast and instantly readable. Light it with soft volumetric beams and low atmospheric haze, show visible brushwork in the sky and the crowd, and add luminous accents. Give the characters graceful, slightly elongated proportions and expressive faces. Frame it cinematically, with a shallow depth of field.

Everything here is an original design invented for this game: the people, kits, ball, stadium and crowd are all new. Kits are plain fabric with abstract patterns, the ball carries only its panel pattern, and boards and signs are blank or abstract. Keep the whole image free of text, numbers, logos and watermarks.
```

## What I'll build in Blender

**Scenes**

- **The stare-down arena.** The first playable world: the floating night arena, its stands, crowd, sky and lighting, with the phone cameras for kicker, keeper and the stare-down close-up.
- **The free-kick set piece.** The same arena with a wall at the 9.15 m spray line and the referee.
- **Two more eras.** The 1970s sun-bleached afternoon and the 1990s floodlit neon night, each lit and graded as its own world.
- **Classics scenes.** Clifftop, storm, frozen lake, monsoon and above the clouds, one per scenario card.

**Animations**

- **Kicker.** Ready idle, run-up, stutter, side-foot, driven knuckleball, curler, trivela, chip, two celebrations, a miss.
- **Keeper.** Ready stance, shuffle, low, middle and top-corner dives to both sides, staying big, catch, smother, fingertip save, punch, celebration, despair.
- **Wall.** Line up, jump, brace, the lying man.
- **Ball and effects.** Light ribbons that follow flight paths exported from the game's physics, the net bursting into light, crowd waves.

You get `.blend` scenes, FBX characters and animations ready for Unity, and preview renders and videos along the way.

## Sending things back

- **Images:** save each under the name on its prompt (alternatives get `-v2`), then attach them in our chat or upload them to their folder under `art/reference/`.
- **Models:** any format Blender opens: `.blend`, `.fbx`, `.glb`/`.gltf` or `.obj`. Metres as units is ideal. A humanoid rig is a bonus; I can rig unrigged characters.
- **Rights:** only send models and images you made or are allowed to use in a commercial game, and tell me any credit a licence requires.
