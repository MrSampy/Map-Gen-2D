let functionarr = [];

function drawRect(x, y, red, green, blue, lenofpix) {
    let canvas = document.getElementById("canvas");
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
