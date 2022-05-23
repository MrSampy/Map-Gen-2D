let functionarr = [];
const canvs = ["canvas1","canvas2","canvas3`"];
function drawRect(x, y, red, green, blue, lenofpix,num) {
    let canvas = document.getElementById(canvs[num]);
    if (canvas.getContext) {
        let ctx = canvas.getContext("2d");
        ctx.fillStyle = `rgb(${red},${green},${blue})`;
        ctx.fillRect(x, y, lenofpix, lenofpix);
    }
}
function drawmap() {
    for (const func of functionarr)
        func();

}
function getImage(canvas){
    var imageData = canvas.toDataURL();
    var image = new Image();
    image.src = imageData;
    return image;
}

function saveImage(image) {
    var link = document.createElement("a");

    link.setAttribute("href", image.src);
    link.setAttribute("download", "123");
    link.click();
}

function saveCanvasAsImageFile(num){
    var image = getImage(document.getElementById(canvs[num]));
    saveImage(image);
}