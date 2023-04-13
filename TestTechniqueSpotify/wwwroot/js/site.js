// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const popup = document.getElementById("Info");

// Close the popup on click outside
popup.addEventListener("click", function (event) {
    if (event.target === popup) {
        location.href = "#";
    }
});
