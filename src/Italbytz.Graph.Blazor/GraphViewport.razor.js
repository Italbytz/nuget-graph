const stateMap = new WeakMap();

export function initialize(host) {
    if (!host || stateMap.has(host)) {
        return;
    }

    const stage = host.querySelector('[data-graph-stage]');
    const svg = host.querySelector('svg');
    const viewport = host.querySelector('[data-graph-viewport]');

    if (!stage || !svg || !viewport) {
        return;
    }

    const state = {
        host,
        stage,
        svg,
        viewport,
        scale: 1,
        translateX: 0,
        translateY: 0,
        pointerId: null,
        lastX: 0,
        lastY: 0,
        fitted: false,
        resizeObserver: null,
        onWheel: null,
        onPointerDown: null,
        onPointerMove: null,
        onPointerUp: null,
        onDoubleClick: null,
        onKeyDown: null
    };

    state.onWheel = event => {
        event.preventDefault();
        const point = clientToSvgPoint(state.svg, event.clientX, event.clientY);
        const factor = event.deltaY < 0 ? 1.12 : 0.88;
        zoomAt(state, point.x, point.y, factor);
    };

    state.onPointerDown = event => {
        if (event.button !== 0) {
            return;
        }

        state.pointerId = event.pointerId;
        state.lastX = event.clientX;
        state.lastY = event.clientY;
        state.stage.classList.add('graph-viewport-dragging');
        state.stage.setPointerCapture(event.pointerId);
    };

    state.onPointerMove = event => {
        if (state.pointerId !== event.pointerId) {
            return;
        }

        const viewBox = state.svg.viewBox.baseVal;
        const bounds = state.stage.getBoundingClientRect();
        const deltaX = ((event.clientX - state.lastX) / bounds.width) * viewBox.width;
        const deltaY = ((event.clientY - state.lastY) / bounds.height) * viewBox.height;

        state.translateX += deltaX;
        state.translateY += deltaY;
        state.lastX = event.clientX;
        state.lastY = event.clientY;

        applyTransform(state);
    };

    state.onPointerUp = event => {
        if (state.pointerId !== event.pointerId) {
            return;
        }

        state.pointerId = null;
        state.stage.classList.remove('graph-viewport-dragging');
        if (state.stage.hasPointerCapture(event.pointerId)) {
            state.stage.releasePointerCapture(event.pointerId);
        }
    };

    state.onDoubleClick = event => {
        event.preventDefault();
        fit(host);
    };

    state.onKeyDown = event => {
        switch (event.key) {
            case '+':
            case '=':
                event.preventDefault();
                zoomIn(host);
                break;
            case '-':
            case '_':
                event.preventDefault();
                zoomOut(host);
                break;
            case '0':
                event.preventDefault();
                reset(host);
                break;
            case 'f':
            case 'F':
                event.preventDefault();
                fit(host);
                break;
            case 'ArrowLeft':
                event.preventDefault();
                nudge(state, 18, 0);
                break;
            case 'ArrowRight':
                event.preventDefault();
                nudge(state, -18, 0);
                break;
            case 'ArrowUp':
                event.preventDefault();
                nudge(state, 0, 18);
                break;
            case 'ArrowDown':
                event.preventDefault();
                nudge(state, 0, -18);
                break;
        }
    };

    stage.addEventListener('wheel', state.onWheel, { passive: false });
    stage.addEventListener('pointerdown', state.onPointerDown);
    stage.addEventListener('pointermove', state.onPointerMove);
    stage.addEventListener('pointerup', state.onPointerUp);
    stage.addEventListener('pointerleave', state.onPointerUp);
    stage.addEventListener('dblclick', state.onDoubleClick);
    stage.addEventListener('keydown', state.onKeyDown);

    state.resizeObserver = new ResizeObserver(() => {
        if (!state.pointerId) {
            fit(host);
        }
    });
    state.resizeObserver.observe(stage);

    stateMap.set(host, state);
    fit(host);
}

export function refresh(host) {
    const state = stateMap.get(host);
    if (!state) {
        return;
    }

    state.viewport = host.querySelector('[data-graph-viewport]');
    state.svg = host.querySelector('svg');
    if (!state.fitted) {
        fit(host);
        return;
    }

    applyTransform(state);
}

export function zoomIn(host) {
    const state = stateMap.get(host);
    if (!state) {
        return;
    }

    const viewBox = state.svg.viewBox.baseVal;
    zoomAt(state, viewBox.width / 2, viewBox.height / 2, 1.15);
}

export function zoomOut(host) {
    const state = stateMap.get(host);
    if (!state) {
        return;
    }

    const viewBox = state.svg.viewBox.baseVal;
    zoomAt(state, viewBox.width / 2, viewBox.height / 2, 0.87);
}

export function fit(host) {
    const state = stateMap.get(host);
    if (!state || !state.viewport) {
        return;
    }

    const bbox = state.viewport.getBBox();
    if (!bbox || bbox.width <= 0 || bbox.height <= 0) {
        return;
    }

    const viewBox = state.svg.viewBox.baseVal;
    const padding = 32;
    const scaleX = (viewBox.width - (padding * 2)) / bbox.width;
    const scaleY = (viewBox.height - (padding * 2)) / bbox.height;
    state.scale = clamp(Math.min(scaleX, scaleY), 0.55, 2.8);
    state.translateX = (viewBox.width / 2) - ((bbox.x + (bbox.width / 2)) * state.scale);
    state.translateY = (viewBox.height / 2) - ((bbox.y + (bbox.height / 2)) * state.scale);
    state.fitted = true;

    applyTransform(state);
}

export function reset(host) {
    fit(host);
}

export function dispose(host) {
    const state = stateMap.get(host);
    if (!state) {
        return;
    }

    state.stage.removeEventListener('wheel', state.onWheel);
    state.stage.removeEventListener('pointerdown', state.onPointerDown);
    state.stage.removeEventListener('pointermove', state.onPointerMove);
    state.stage.removeEventListener('pointerup', state.onPointerUp);
    state.stage.removeEventListener('pointerleave', state.onPointerUp);
    state.stage.removeEventListener('dblclick', state.onDoubleClick);
    state.stage.removeEventListener('keydown', state.onKeyDown);
    state.resizeObserver?.disconnect();
    stateMap.delete(host);
}

function nudge(state, deltaX, deltaY) {
    state.translateX += deltaX;
    state.translateY += deltaY;
    applyTransform(state);
}

function zoomAt(state, anchorX, anchorY, factor) {
    const previousScale = state.scale;
    const nextScale = clamp(previousScale * factor, 0.45, 3.5);

    if (Math.abs(previousScale - nextScale) < 0.0001) {
        return;
    }

    state.translateX = anchorX - (((anchorX - state.translateX) / previousScale) * nextScale);
    state.translateY = anchorY - (((anchorY - state.translateY) / previousScale) * nextScale);
    state.scale = nextScale;
    applyTransform(state);
}

function applyTransform(state) {
    if (!state.viewport) {
        return;
    }

    state.viewport.setAttribute('transform', `translate(${state.translateX} ${state.translateY}) scale(${state.scale})`);
}

function clientToSvgPoint(svg, clientX, clientY) {
    const point = svg.createSVGPoint();
    point.x = clientX;
    point.y = clientY;
    const matrix = svg.getScreenCTM();
    return matrix ? point.matrixTransform(matrix.inverse()) : { x: 0, y: 0 };
}

function clamp(value, min, max) {
    return Math.min(Math.max(value, min), max);
}