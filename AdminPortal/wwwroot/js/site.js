// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * 初始化水印
 */
function InitWatermark(event) {
    const data = event.currentTarget.dataset;
    const watermark = new WatermarkPlus.Watermark({
        contentType: 'multi-line-text',
        content: 'AdminPortal \n' + data.text,
        width: 500,
        height: 200,
        rotate: 22,
        layout: 'grid',
        globalAlpha: 0.1,
        // textType: 'stroke',
        lineHeight: 30,
        fontSize: '25px',
        gridLayoutOptions: {
            rows: 2,
            cols: 2,
            gap: [20, 20],
            matrix: [
                [1, 0],
                [0, 1],
            ],
        },
    })
    watermark.create();
}
