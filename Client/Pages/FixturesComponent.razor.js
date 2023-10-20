let firstExecution = true;
function scrollToElement(elementId) {
    if (firstExecution === true) {
        var element = document.getElementById(elementId);
        if (element) {
            element.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
        }
    }
    firstExecution = false;
}

export { firstExecution, scrollToElement };