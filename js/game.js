var WasmTetris;
(function (WasmTetris) {
    class RenderEngine {
        lastRender = 0;
        fpsCount = 0;
        renderContext;
        canvasWidth;
        canvasHeight;
        fpsCalculator = new WasmTetris.FpsCalculator();
        imageLoader = new WasmTetris.ImageLoader();
        renderEngine;
        constructor(renderEngine) {
            this.renderEngine = renderEngine;
        }
        async loadImages(imageUrls) {
            await this.imageLoader.loadImages(imageUrls);
        }
        async startEngine() {
            let canvas = document.querySelector("canvas");
            canvas.width = window.innerWidth - 6;
            canvas.height = window.innerHeight - 6;
            this.renderContext = canvas.getContext("2d");
            this.canvasWidth = canvas.width;
            this.canvasHeight = canvas.height;
            window.requestAnimationFrame(this.loop);
        }
        drawImage(imageUrl, posX, posY) {
            let image = this.imageLoader.getImage(imageUrl);
            if (image !== undefined) {
                this.renderContext.drawImage(this.imageLoader.getImage(imageUrl), posX, posY);
            }
        }
        update(deltaTime) {
            this.renderContext.clearRect(0, 0, this.canvasWidth, this.canvasHeight);
            this.renderEngine.invokeMethodAsync("UpdateGameObjects", deltaTime / 100.0);
            this.showFPS();
        }
        showFPS() {
            this.renderContext.fillStyle = "Black";
            this.renderContext.font = "normal 16pt Arial";
            this.renderContext.fillText(this.fpsCount + " fps", 10, 26);
        }
        loop = (timestamp) => {
            var progress = timestamp - this.lastRender;
            this.update(progress);
            this.fpsCount = this.fpsCalculator.tick(timestamp);
            this.lastRender = timestamp;
            window.requestAnimationFrame(this.loop);
        };
    }
    WasmTetris.RenderEngine = RenderEngine;
    function createRenderEngineInstance(renderEngine) {
        return new RenderEngine(renderEngine);
    }
    WasmTetris.createRenderEngineInstance = createRenderEngineInstance;
})(WasmTetris || (WasmTetris = {}));
var WasmTetris;
(function (WasmTetris) {
    class FpsCalculator {
        sampleSize = 60;
        sampleData = Array();
        index = 0;
        lastTick = 0;
        tick(timestamp) {
            if (this.lastTick === 0) {
                this.lastTick = timestamp;
                return 0;
            }
            let now = performance.now();
            let delta = (now - this.lastTick) / 1000;
            let fps = 1 / delta;
            this.sampleData[this.index] = Math.round(fps);
            let average = this.sampleData.reduce((a, b) => a + b, 0);
            average = Math.round(average / this.sampleData.length);
            this.lastTick = now;
            this.index++;
            if (this.index === this.sampleSize)
                this.index = 0;
            return average;
        }
    }
    WasmTetris.FpsCalculator = FpsCalculator;
})(WasmTetris || (WasmTetris = {}));
var WasmTetris;
(function (WasmTetris) {
    class ImageLoader {
        imageSources = {};
        loadImages(imageUrls) {
            return new Promise(async (r) => {
                await Promise.all(imageUrls.map(async (imageUrl) => {
                    let newImage = new Image();
                    let prom = new Promise(r => newImage.onload = r);
                    newImage.src = imageUrl;
                    await prom;
                    this.imageSources[imageUrl] = newImage;
                }));
                r(0);
            });
        }
        getImage(imageUrl) {
            return this.imageSources[imageUrl];
        }
    }
    WasmTetris.ImageLoader = ImageLoader;
})(WasmTetris || (WasmTetris = {}));
//# sourceMappingURL=game.js.map