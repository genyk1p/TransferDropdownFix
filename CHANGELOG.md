# Changelog

## 0.1.2 — 2026-10-16
- First public release: the closed drop-downs of the Konstruction resource transfer window show the selected vessel names again (caption vertical overflow set to Overflow).
- Before release the mod was called KonstructionFix (MIT); now Transfer Dropdown Fix, GPL v3. Same code.

## 0.1.1 — not released
- Removed the diagnostic logging of the drop-down setup and the "wire a missing caption" fallback; only the fix is left. One line in `KSP.log` at startup tells that the window is patched.

## 0.1.0 — not released
- Resource transfer window: vessel names were not shown in the closed drop-downs. Cause (from the logged setup): the caption rect is 17 px high for Arial 16 and Unity drops a line that does not fit (vertical overflow Truncate); the caption overflow is set to Overflow. Also wires a missing caption, just in case.
