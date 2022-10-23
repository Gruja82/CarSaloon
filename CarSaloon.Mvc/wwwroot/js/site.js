function setSrc() {
    let img = document.getElementById("imgControl");
    const [inputValue] = document.getElementById("inputControl").files;
    if (inputValue) {
        img.src = URL.createObjectURL(inputValue);
    }
}