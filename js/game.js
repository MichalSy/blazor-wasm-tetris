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
        canvasElement;
        constructor(renderEngine) {
            this.renderEngine = renderEngine;
            this.canvasElement = document.querySelector("canvas");
            this.renderContext = this.canvasElement.getContext("2d");
            window.addEventListener("resize", () => this.detectWindowSize());
        }
        detectWindowSize() {
            this.renderEngine.invokeMethodAsync("SetWindowSize", window.innerWidth, window.innerHeight);
        }
        setCanvasSize(width, height) {
            this.canvasWidth = width;
            this.canvasHeight = height;
            this.canvasElement.width = this.canvasWidth;
            this.canvasElement.height = this.canvasHeight;
        }
        async loadImages(imageUrls) {
            await this.imageLoader.loadImages(imageUrls);
        }
        async startEngine() {
            this.detectWindowSize();
            window.requestAnimationFrame(this.loop);
        }
        drawRectWithBorder(command) {
            let data = command.data;
            this.drawFillRect({
                data: { color: data.color, alpha: 0.75 },
                positionX: command.positionX,
                positionY: command.positionY,
                width: command.width,
                height: command.height
            });
            this.drawStrokeRect({
                data: { lineWidth: 2, alpha: 0.15 },
                positionX: command.positionX,
                positionY: command.positionY,
                width: command.width,
                height: command.height
            });
        }
        drawFillRect(command) {
            let data = command.data;
            this.renderContext.fillStyle = data.color ?? "#000";
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.fillRect(command.positionX, command.positionY, command.width, command.height);
            this.renderContext.globalAlpha = 1;
        }
        drawStrokeRect(command) {
            let data = command.data;
            this.renderContext.strokeStyle = data.color ?? "#000";
            this.renderContext.lineWidth = data.lineWidth ?? 1;
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.strokeRect(command.positionX, command.positionY, command.width, command.height);
            this.renderContext.globalAlpha = 1;
        }
        drawImages(images) {
            for (let i of images) {
                let image = this.imageLoader.getImage(i.imageUrl);
                if (image !== undefined) {
                    this.renderContext.drawImage(this.imageLoader.getImage(i.imageUrl), i.positionX, i.positionY);
                }
            }
        }
        drawObjects(renderObjects) {
            for (let obj of renderObjects) {
                if (obj.type === "Image") {
                    let i = obj.data;
                    let image = this.imageLoader.getImage(i.imageUrl);
                    if (image !== undefined) {
                        this.renderContext.drawImage(this.imageLoader.getImage(i.imageUrl), obj.positionX, obj.positionY);
                    }
                }
                else if (obj.type === "RectWithBorder") {
                    this.drawRectWithBorder(obj);
                }
                else if (obj.type === "FillRect") {
                    this.drawFillRect(obj);
                }
                else if (obj.type === "StrokeRect") {
                    this.drawStrokeRect(obj);
                }
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
//# sourceMappingURL=game.js.map