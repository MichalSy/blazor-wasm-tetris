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
                //this.drawRectWithBorder("#ff0000", posX, posY);
            }
        }

        drawRectWithBorder(color: string, posX: number, posY: number, width: number, height: number) {

            this.renderContext.globalAlpha = 0.7;
            this.renderContext.fillStyle = color;
            this.renderContext.fillRect(posX, posY, width, width);

            this.renderContext.strokeStyle = '#000';
            this.renderContext.lineWidth = 2;
            this.renderContext.globalAlpha = 1;
            this.renderContext.strokeRect(posX, posY, width, height);
            
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
                    let i = <any>obj.data;
                    this.drawRectWithBorder(i.color, obj.positionX, obj.positionY, obj.width, obj.height);
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