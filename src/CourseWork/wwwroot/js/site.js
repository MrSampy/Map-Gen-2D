let functionarr = [];
const canvs = [`canvas1`,`canvas2`,`canvas3`];
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
