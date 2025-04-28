(() => {
    const root = document.documentElement;
    const btn = document.getElementById('themeToggle');

    if (!btn) return; // fails quietly if button missing

    // read last preference from localStorage
    if (localStorage.theme === 'dark') {
        root.classList.add('dark');
    }

    btn.addEventListener('click', () => {
        root.classList.toggle('dark');
        // remember the choice
        localStorage.theme = root.classList.contains('dark') ? 'dark' : 'light';
    });
})();
