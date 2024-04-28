const popoverElements = document.querySelectorAll('[data-bs-toggle="popover"]');

for (const popover of popoverElements) {
    new bootstrap.Popover(popover); // eslint-disable-line no-new
}


