
const canvasId = ["canvas1","canvas2","canvas3","canvas4"]

function drawRect(canvasId, x, y, red, green, blue, lenofpix, num) {
    let ctx = canvasId[num];
    if (!ctx) return;
    ctx.fillStyle = `rgb(${red},${green},${blue})`;
    ctx.fillRect(x, y, lenofpix, lenofpix);
}

function getImage(canvas){
    let imageData = canvas.toDataURL();
    let image = new Image();
    image.src = imageData;
    return image;
}

function saveImage(image) {
    let link = document.createElement("a");

    link.setAttribute("href", image.src);
    link.setAttribute("download", "123");
    link.click();
}

function saveCanvasAsImageFile(num){
    let image = getImage(document.getElementById(canvasId[num]));
    saveImage(image);
}