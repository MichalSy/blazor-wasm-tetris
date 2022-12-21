namespace WasmTetris {
    export class RenderEngine {
        lastRender = 0;
        fpsCount = 0;
        renderContext: CanvasRenderingContext2D;
        canvasWidth: number;
        canvasHeight: number;
        fpsCalculator = new FpsCalculator();
        imageLoader = new ImageLoader();
        renderEngine: any;

        constructor(renderEngine: unknown) {
            this.renderEngine = renderEngine;
        }

        async loadImages(imageUrls: Array<string>) {
            await this.imageLoader.loadImages(imageUrls);
        }

        async startEngine() {
            let canvas = document.querySelector("canvas")
            canvas.width = window.innerWidth - 6;
            canvas.height = window.innerHeight - 6;
            this.renderContext = canvas.getContext("2d");
            this.canvasWidth = canvas.width;
            this.canvasHeight = canvas.height;

            window.requestAnimationFrame(this.loop);
        }

        drawImage(imageUrl: string, posX: number, posY: number) {
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
        }



    }

    export function createRenderEngineInstance(renderEngine): RenderEngine {
        return new RenderEngine(renderEngine);
    }
}