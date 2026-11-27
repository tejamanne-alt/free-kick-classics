"""Renders the pitch layout guides attached to the image prompts.

Each guide shows regulation markings, the goal and where the players stand, seen from one game camera, in flat
placeholder colours. Image generators copy the layout from it; everything else comes from the prompt.

Run headless:  xvfb-run -a blender -b --python art/blender/render_layout_guides.py -- <output dir>
Axes: Z up, the goal line runs along X at Y = 0, the pitch extends toward +Y. Units are metres.
"""
import math
import sys

import bpy
from mathutils import Vector

LINE = 0.12          # line width (Laws of the Game maximum)
GOAL_HALF = 3.66     # inside of post to centre
POST_R = 0.06
BAR_Z = 2.44
HALF_WIDTH = 34.0    # 68 m wide pitch
RUN_OFF = 5.0        # grass between the lines and the perimeter boards

# Display (sRGB) colours, converted to the linear values Blender stores.
SRGB = {
    'grass': (0.24, 0.47, 0.22), 'surround': (0.17, 0.33, 0.17), 'line': (1.0, 1.0, 1.0),
    'goal': (1.0, 1.0, 1.0), 'net': (0.70, 0.70, 0.70), 'board': (0.22, 0.23, 0.26),
    'figure': (0.50, 0.50, 0.54), 'keeper': (0.60, 0.56, 0.48), 'ball': (1.0, 1.0, 1.0),
    'sky': (0.84, 0.85, 0.87),
}


def to_linear(c):
    return c / 12.92 if c <= 0.04045 else ((c + 0.055) / 1.055) ** 2.4


COLOURS = {k: tuple(to_linear(c) for c in v) + (1,) for k, v in SRGB.items()}


def reset():
    bpy.ops.wm.read_factory_settings(use_empty=True)
    scene = bpy.context.scene
    scene.render.engine = 'BLENDER_WORKBENCH'
    shading = scene.display.shading
    shading.light = 'FLAT'
    shading.color_type = 'OBJECT'
    shading.background_type = 'WORLD'
    world = bpy.data.worlds.new('World')
    world.color = COLOURS['sky'][:3]
    scene.world = world
    scene.display.render_aa = '16'
    scene.view_settings.view_transform = 'Standard'
    scene.render.dither_intensity = 0  # flat colours compress far better without dither noise
    return scene


def mesh_object(name, verts, faces, colour):
    mesh = bpy.data.meshes.new(name)
    mesh.from_pydata(verts, [], faces)
    obj = bpy.data.objects.new(name, mesh)
    obj.color = COLOURS[colour]
    bpy.context.collection.objects.link(obj)
    return obj


def strip(name, points, width=LINE, z=0.003, colour='line'):
    """A flat line of constant width along a polyline on the ground."""
    verts, faces = [], []
    for i, (x, y) in enumerate(points):
        a = Vector(points[max(i - 1, 0)])
        b = Vector(points[min(i + 1, len(points) - 1)])
        d = (b - a).normalized()
        n = Vector((-d.y, d.x)) * (width / 2)
        verts += [(x + n.x, y + n.y, z), (x - n.x, y - n.y, z)]
        if i:
            k = 2 * i
            faces.append((k - 2, k - 1, k + 1, k))
    return mesh_object(name, verts, faces, colour)


def disc(name, cx, cy, r, colour='line', z=0.004, steps=32):
    verts = [(cx, cy, z)] + [(cx + r * math.cos(2 * math.pi * i / steps), cy + r * math.sin(2 * math.pi * i / steps), z) for i in range(steps)]
    faces = [(0, i + 1, (i + 1) % steps + 1) for i in range(steps)]
    return mesh_object(name, verts, faces, colour)


def box(name, centre, size, colour):
    bpy.ops.mesh.primitive_cube_add(size=1, location=centre)
    obj = bpy.context.object
    obj.name = name
    obj.scale = size
    obj.color = COLOURS[colour]
    return obj


def cylinder(name, a, b, r, colour):
    a, b = Vector(a), Vector(b)
    bpy.ops.mesh.primitive_cylinder_add(radius=r, depth=(b - a).length, location=(a + b) / 2, vertices=24)
    obj = bpy.context.object
    obj.name = name
    obj.rotation_mode = 'QUATERNION'
    obj.rotation_quaternion = Vector((0, 0, 1)).rotation_difference((b - a).normalized())
    obj.color = COLOURS[colour]
    return obj


def figure(name, x, y, colour='figure', height=1.82, crouch=False):
    """A plain capsule mannequin marking where a player stands."""
    h = height * (0.8 if crouch else 1.0)
    w = 0.75 if crouch else 0.42
    box(name + '_body', (x, y, h * 0.45), (w, 0.28, h * 0.82), colour)
    bpy.ops.mesh.primitive_uv_sphere_add(radius=0.12, location=(x, y, h * 0.93))
    bpy.context.object.color = COLOURS[colour]


LENGTH = 105.0       # goal line to goal line


def arc_points(cx, cy, r, a0, a1, steps=64):
    return [(cx + r * math.cos(a0 + (a1 - a0) * t / steps), cy + r * math.sin(a0 + (a1 - a0) * t / steps)) for t in range(steps + 1)]


def build_end(tag, far):
    """Markings and goal for one end. The near end's goal line is Y = 0; the far end is mirrored at Y = LENGTH."""
    f = (lambda x, y: (x, LENGTH - y)) if far else (lambda x, y: (x, y))
    m = lambda pts: [f(x, y) for x, y in pts]
    h = LINE / 2
    strip(f'{tag}_goal_line', m([(-HALF_WIDTH, h), (HALF_WIDTH, h)]))
    for name, half, depth in (('goal_area', GOAL_HALF + 5.5, 5.5), ('penalty_area', GOAL_HALF + 16.5, 16.5)):
        strip(f'{tag}_{name}_front', m([(-half - h, depth - h), (half + h, depth - h)]))
        strip(f'{tag}_{name}_left', m([(-half + h, 0), (-half + h, depth)]))
        strip(f'{tag}_{name}_right', m([(half - h, 0), (half - h, depth)]))
    x, y = f(0, 11)
    disc(f'{tag}_penalty_spot', x, y, 0.11)
    # The arc: the part of a 9.15 m circle around the spot outside the penalty area.
    a = math.asin(5.5 / 9.15)
    strip(f'{tag}_penalty_arc', m(arc_points(0, 11, 9.15 - h, a, math.pi - a)))
    strip(f'{tag}_corner_left', m(arc_points(-HALF_WIDTH, 0, 1.0, 0, math.pi / 2, 16)))
    strip(f'{tag}_corner_right', m(arc_points(HALF_WIDTH, 0, 1.0, math.pi / 2, math.pi, 16)))
    # Goal: posts and bar on the goal line, net supports behind.
    px = GOAL_HALF + POST_R
    g = lambda x, y, z: (*f(x, y), z)
    for side in (-1, 1):
        cylinder(f'{tag}_post_{side}', g(side * px, POST_R, 0), g(side * px, POST_R, BAR_Z + 2 * POST_R), POST_R, 'goal')
        cylinder(f'{tag}_net_top_{side}', g(side * px, POST_R, BAR_Z), g(side * px, -1.5, BAR_Z - 0.4), 0.02, 'net')
        cylinder(f'{tag}_net_back_{side}', g(side * px, -1.5, BAR_Z - 0.4), g(side * px, -2.0, 0), 0.02, 'net')
        cylinder(f'{tag}_flag_{side}', g(side * HALF_WIDTH, 0, 0), g(side * HALF_WIDTH, 0, 1.5), 0.02, 'goal')
    cylinder(f'{tag}_bar', g(-px, POST_R, BAR_Z + POST_R), g(px, POST_R, BAR_Z + POST_R), POST_R, 'goal')
    cylinder(f'{tag}_net_back_top', g(-px, -1.5, BAR_Z - 0.4), g(px, -1.5, BAR_Z - 0.4), 0.02, 'net')
    cylinder(f'{tag}_net_ground', g(-px, -2.0, 0.02), g(px, -2.0, 0.02), 0.02, 'net')
    bx, by = f(0, -RUN_OFF - 0.1)
    box(f'{tag}_boards', (bx, by, 0.45), (2 * (HALF_WIDTH + RUN_OFF), 0.2, 0.9), 'board')


def build_goal_end(spray_line=None):
    """The full 105 x 68 m pitch with run-off and perimeter boards; the near goal is at Y = 0."""
    mesh_object('surround', [(-90, -70, -0.01), (90, -70, -0.01), (90, LENGTH + 70, -0.01), (-90, LENGTH + 70, -0.01)], [(0, 1, 2, 3)], 'surround')
    mesh_object('grass', [(-HALF_WIDTH - RUN_OFF, -RUN_OFF, 0), (HALF_WIDTH + RUN_OFF, -RUN_OFF, 0), (HALF_WIDTH + RUN_OFF, LENGTH + RUN_OFF, 0), (-HALF_WIDTH - RUN_OFF, LENGTH + RUN_OFF, 0)], [(0, 1, 2, 3)], 'grass')
    h = LINE / 2
    strip('touch_left', [(-HALF_WIDTH + h, 0), (-HALF_WIDTH + h, LENGTH)])
    strip('touch_right', [(HALF_WIDTH - h, 0), (HALF_WIDTH - h, LENGTH)])
    strip('halfway', [(-HALF_WIDTH, LENGTH / 2), (HALF_WIDTH, LENGTH / 2)])
    disc('centre_spot', 0, LENGTH / 2, 0.11)
    strip('centre_circle', arc_points(0, LENGTH / 2, 9.15 - h, 0, 2 * math.pi, 128))
    build_end('near', far=False)
    build_end('far', far=True)
    for side in (-1, 1):
        box(f'boards_side_{side}', (side * (HALF_WIDTH + RUN_OFF + 0.1), LENGTH / 2, 0.45), (0.2, LENGTH + 2 * RUN_OFF, 0.9), 'board')
    if spray_line:
        strip('spray', spray_line, width=0.08, z=0.005)


def camera(location, target, lens, ortho_scale=None):
    bpy.ops.object.camera_add(location=location)
    cam = bpy.context.object
    direction = Vector(target) - Vector(location)
    cam.rotation_euler = direction.to_track_quat('-Z', 'Y').to_euler()
    cam.data.sensor_width = 36
    if ortho_scale:
        cam.data.type = 'ORTHO'
        cam.data.ortho_scale = ortho_scale
    else:
        cam.data.lens = lens
    cam.data.clip_end = 500
    bpy.context.scene.camera = cam


def render(path, width, height):
    scene = bpy.context.scene
    scene.render.resolution_x, scene.render.resolution_y = width, height
    scene.render.filepath = path
    settings = scene.render.image_settings
    settings.file_format, settings.color_mode, settings.color_depth, settings.compression = 'PNG', 'RGB', '8', 100
    bpy.ops.render.render(write_still=True)


def penalty_players(kicker_x=1.3, kicker_y=13.6):
    # +X is the kicker's left (and screen left from behind him): a right-footer starts there.
    bpy.ops.mesh.primitive_uv_sphere_add(radius=0.11, location=(0, 11, 0.11))
    bpy.context.object.color = COLOURS['ball']
    figure('kicker', kicker_x, kicker_y)
    figure('keeper', 0, 0.35, 'keeper', crouch=True)


GUIDES = {
    'G01-penalty-kicker-view': dict(size=(1536, 1024), cam=((-2.0, 27.0, 2.6), (0.3, 0.0, 0.6), 32), players='penalty'),
    'G02-penalty-game-camera': dict(size=(1024, 1536), cam=((0.0, 18.5, 4.2), (0.0, 2.0, 0.6), 34), players='penalty_centre'),
    'G03-keeper-view': dict(size=(1024, 1536), cam=((0.0, 0.9, 1.75), (0.0, 20.0, -0.6), 30), players='penalty_centre'),
    'G04-goal-end-plan': dict(size=(1024, 1024), cam=((0.0, 12.0, 80.0), (0.0, 12.0, 0.0), None, 80.0), players='none'),
    'G05-free-kick-view': dict(size=(1536, 1024), cam=((-9.6, 43.0, 2.6), (1.5, 0.0, 0.9), 30), players='free_kick'),
    'G06-stadium-aerial': dict(size=(1536, 1024), cam=((-46.0, -30.0, 42.0), (2.0, 30.0, 0.0), 30), players='penalty'),
}


def free_kick_players():
    ball = Vector((-6.0, 34.5))  # 35 m from the centre of the goal
    goal = Vector((0.0, 0.0))
    to_goal = (goal - ball).normalized()
    side = Vector((-to_goal.y, to_goal.x))
    wall_centre = ball + to_goal * 9.15
    bpy.ops.mesh.primitive_uv_sphere_add(radius=0.11, location=(ball.x, ball.y, 0.11))
    bpy.context.object.color = COLOURS['ball']
    for i in range(6):
        p = wall_centre + side * ((i - 2.5) * 0.55)
        figure(f'wall_{i}', p.x, p.y)
    kicker = ball - to_goal * 2.6 + side * 0.9
    figure('kicker', kicker.x, kicker.y)
    figure('keeper', 1.2, 0.6, 'keeper', crouch=True)
    a = wall_centre + side * 2.3
    b = wall_centre - side * 2.3
    return [(a.x, a.y), (b.x, b.y)]


def main(out_dir):
    for name, spec in GUIDES.items():
        reset()
        spray = None
        if spec['players'] == 'free_kick':
            build_goal_end()
            spray = free_kick_players()
            strip('spray', spray, width=0.08, z=0.005)
        else:
            build_goal_end()
            if spec['players'] == 'penalty':
                penalty_players()
            elif spec['players'] == 'penalty_centre':
                penalty_players(kicker_x=0.9, kicker_y=13.3)
        location, target, lens, *ortho = spec['cam']
        camera(location, target, lens, ortho[0] if ortho else None)
        render(f'{out_dir}/{name}.png', *spec['size'])
        print('RENDERED', name)


if __name__ == '__main__':
    argv = sys.argv[sys.argv.index('--') + 1:] if '--' in sys.argv else ['.']
    main(argv[0])
