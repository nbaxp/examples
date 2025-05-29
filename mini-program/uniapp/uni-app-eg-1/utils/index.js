export function delay(ms = 1000) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

export function debounce(func, wait = 300) {
    let timeout
    return function(...args) {
        const context = this
        clearTimeout(timeout)
        timeout = setTimeout(() => {
            func.apply(context, args)
        }, wait)
    }
}

export function formatDistance(meters) {
    if (meters >= 1000) {
        return (meters / 1000).toFixed(2) + '千米';
    } else {
        return Math.round(meters) + '米';
    }
}