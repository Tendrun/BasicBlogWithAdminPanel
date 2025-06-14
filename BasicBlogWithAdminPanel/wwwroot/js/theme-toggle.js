// -----------------------------------------------------------
// Dark-mode toggle – permanent moon icon
// -----------------------------------------------------------
document.addEventListener('DOMContentLoaded', () => {

    const toggleBtn = document.getElementById('themeToggle');
    const iconSpan = document.getElementById('themeIcon');

    if (!toggleBtn || !iconSpan) {
        console.warn('[theme-toggle] Button or icon <span> not found.');
        return;
    }

    /* moon forever */
    iconSpan.textContent = '🌜';

    const DARK = 'dark';
    const LIGHT = 'light';
    const STORE = 'bbap-theme';
    let current;

    const apply = (theme) => {
        if (theme === DARK) {
            document.documentElement.classList.add(DARK);
        } else {
            document.documentElement.classList.remove(DARK);
        }
        localStorage.setItem(STORE, theme);
        current = theme;
    };

    /* initial */
    const stored = localStorage.getItem(STORE);
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    current = stored ?? (prefersDark ? DARK : LIGHT);
    apply(current);

    /* toggle */
    toggleBtn.addEventListener('click', () => {
        apply(current === DARK ? LIGHT : DARK);
    });
});
