function scrollToElement(elementId) {
    var element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
    }
}

export { scrollToElement };