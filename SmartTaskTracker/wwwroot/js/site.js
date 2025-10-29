// Theme toggle using data-bs-theme and localStorage
(function () {
  const themeKey = 'stt-theme';
  const html = document.documentElement;
  const btn = document.getElementById('themeToggle');

  function applyTheme(theme) {
    html.setAttribute('data-bs-theme', theme);
    try { localStorage.setItem(themeKey, theme); } catch { }
    if (btn) {
      const icon = btn.querySelector('i');
      if (icon) {
        icon.className = theme === 'dark' ? 'fa-solid fa-sun' : 'fa-solid fa-moon';
      }
    }
  }

  const stored = (function(){ try { return localStorage.getItem(themeKey); } catch { return null; }})();
  applyTheme(stored || 'light');

  if (btn) {
    btn.addEventListener('click', function(){
      const current = html.getAttribute('data-bs-theme') || 'light';
      applyTheme(current === 'light' ? 'dark' : 'light');
    });
  }
})();

// Animated clock in header
(function(){
  function tick(){
    var el = document.getElementById('sttClock');
    if (!el) return;
    var now = new Date();
    var options = { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' };
    el.textContent = now.toLocaleTimeString([], {hour:'2-digit', minute:'2-digit'}) + ' • ' + now.toLocaleDateString([], options);
  }
  setInterval(tick, 1000);
  tick();
})();



