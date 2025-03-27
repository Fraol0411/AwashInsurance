document.addEventListener("DOMContentLoaded", function () {
    // Add fade-in effect when page loads
    document.body.classList.add("fade-in");

    // Handle all navigation clicks
    document.querySelectorAll("a").forEach(link => {
        link.addEventListener("click", function (event) {
            const href = this.getAttribute("href");
            
            if (href && !href.startsWith("#")) { // Ignore anchor links
                event.preventDefault();
                document.body.classList.add("fade-out");

                setTimeout(() => {
                    window.location.href = href;
                }, 500); // Match the CSS transition time
            }
        });
    });
});
