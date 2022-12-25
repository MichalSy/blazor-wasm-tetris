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

        canvasElement: HTMLCanvasElement;

        constructor(renderEngine: unknown) {
            this.renderEngine = renderEngine;

            this.canvasElement = document.querySelector("canvas")
            this.renderContext = this.canvasElement.getContext("2d");

            window.addEventListener("resize", () => this.detectWindowSize());
        }

        detectWindowSize() {
            this.renderEngine.invokeMethodAsync("SetWindowSize", window.innerWidth, window.innerHeight);
        }

        setCanvasSize(width: number, height: number) {
            this.canvasWidth = width;
            this.canvasHeight = height;
            this.canvasElement.width = this.canvasWidth;
            this.canvasElement.height = this.canvasHeight;
        }

        async loadImages(imageUrls: Array<string>) {
            await this.imageLoader.loadImages(imageUrls);
        }

        async startEngine() {
            this.detectWindowSize();

            window.requestAnimationFrame(this.loop);
        }

        private drawRectWithBorder(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            this.drawFillRect({
                data: { color: data.color, alpha: 0.75 },
                positionX: command.positionX,
                positionY: command.positionY,
                width: command.width,
                height: command.height
            })

            this.drawStrokeRect({
                data: { lineWidth: 2, alpha: 0.15 },
                positionX: command.positionX,
                positionY: command.positionY,
                width: command.width,
                height: command.height
            })
        }

        private drawFillRect(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            this.renderContext.fillStyle = data.color ?? "#000";
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.fillRect(command.positionX, command.positionY, command.width, command.height);
            this.renderContext.globalAlpha = 1;
        }

        private drawStrokeRect(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            this.renderContext.strokeStyle = data.color ?? "#000";
            this.renderContext.lineWidth = data.lineWidth ?? 1;
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.strokeRect(command.positionX, command.positionY, command.width, command.height);
            this.renderContext.globalAlpha = 1;
        }

        drawImages(images: Array<{ imageUrl: string, positionX: number, positionY: number }>) {
            for (let i of images) {
                let image = this.imageLoader.getImage(i.imageUrl);
                if (image !== undefined) {
                    this.renderContext.drawImage(this.imageLoader.getImage(i.imageUrl), i.positionX, i.positionY);
                }
            }
        }

        drawObjects(renderObjects: Array<{ data: unknown, type: string, positionX: number, positionY: number, width: number, height: number }>) {

            for (let obj of renderObjects) {
                if (obj.type === "Image") {
                    let i = <any>obj.data;
                    let image = this.imageLoader.getImage(i.imageUrl);
                    if (image !== undefined) {
                        this.renderContext.drawImage(this.imageLoader.getImage(i.imageUrl), obj.positionX, obj.positionY);
                    }
                } else if (obj.type === "RectWithBorder") {
                    this.drawRectWithBorder(obj);
                } else if (obj.type === "FillRect") {
                    this.drawFillRect(obj);
                } else if (obj.type === "StrokeRect") {
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
        }



    }

    export function createRenderEngineInstance(renderEngine): RenderEngine {
        return new RenderEngine(renderEngine);
    }
}