function buttonOnMouseOver() {
    var element = document.getElementById("nextButton");

    element.style.cursor = "pointer";
    element.classList.add("buttonHoverAnimation");
}

function buttonOnMouseLeave() {
    var element = document.getElementById("nextButton");

    element.classList.remove("buttonHoverAnimation");
    void element.offsetWidth;
}

function buttonOnMouseDown() {
    var element = document.getElementById("nextButton");

    element.classList.add("buttonClickAnimation");
}

function buttonOnMouseUp() {
    var element = document.getElementById("nextButton");

    element.classList.remove("buttonClickAnimation");
    void element.offsetWidth;
}

var view = Windows.UI.ViewManagement.ApplicationView.getForCurrentView();