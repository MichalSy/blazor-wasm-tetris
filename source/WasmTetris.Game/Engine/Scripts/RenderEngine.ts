namespace WasmTetris {
    export class RenderEngine {
        lastRender = 0;
        fpsCount = 0;
        renderContext: CanvasRenderingContext2D;
        canvasWidth: number;
        canvasHeight: number;
        fpsCalculator = new FpsCalculator();
        imageLoader = new ImageLoader();
        soundLoader = new SoundsLoader();
        renderEngine: any;

        canvasElement: HTMLCanvasElement;

        constructor(renderEngine: unknown) {
            this.renderEngine = renderEngine;

            this.canvasElement = document.querySelector("canvas")
            this.renderContext = this.canvasElement.getContext("2d");

            window.addEventListener("resize", () => this.detectWindowSize());
            window.addEventListener("keydown", (event) => this.renderEngine.invokeMethodAsync("SendKeyUpdate", event.type, event.keyCode));
            window.addEventListener("keyup", (event) => this.renderEngine.invokeMethodAsync("SendKeyUpdate", event.type, event.keyCode));

            window.addEventListener("touchstart", (event) => this.renderEngine.invokeMethodAsync("SendTouchUpdate", event.type, Math.round(event.changedTouches[0].clientX), Math.round(event.changedTouches[0].clientY)));
            window.addEventListener("touchend", (event) => this.renderEngine.invokeMethodAsync("SendTouchUpdate", event.type, Math.round(event.changedTouches[0].clientX), Math.round(event.changedTouches[0].clientY)));
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

        async loadSounds(soundUrls: Array<string>) {
            await this.soundLoader.loadSounds(soundUrls);
        }

        async startEngine() {
            this.detectWindowSize();

            window.requestAnimationFrame(this.loop);
        }

        private drawRectWithBorder(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            this.drawFillRect({
                data: { color: data.color, alpha: 1 },
                positionX: command.positionX,
                positionY: command.positionY,
                width: command.width,
                height: command.height
            });

            //this.drawStrokeRect({
            //    data: { color: data.color, lineWidth: 6, alpha: 0.8 },
            //    positionX: command.positionX,
            //    positionY: command.positionY,
            //    width: command.width,
            //    height: command.height
            //});
        }

        private drawFillRect(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            //this.renderContext.globalCompositeOperation = "lighter";
            this.renderContext.shadowColor = data.color ?? "#000";
            this.renderContext.shadowBlur = data.shadowBlur ?? 0;

            this.renderContext.fillStyle = data.color ?? "#000";
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.fillRect(command.positionX, command.positionY, command.width, command.height);
            this.renderContext.globalAlpha = 1;
            this.renderContext.shadowBlur = 0;
            this.renderContext.globalCompositeOperation = "source-over";
        }

        private drawStrokeRect(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            //this.renderContext.globalCompositeOperation = "lighter";
            this.renderContext.shadowColor = data.color ?? "#000";
            this.renderContext.shadowBlur = data.shadowBlur ?? 0;

            this.renderContext.strokeStyle = data.color ?? "#000";
            this.renderContext.lineWidth = data.lineWidth ?? 1;
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.strokeRect(command.positionX, command.positionY, command.width, command.height);
            this.renderContext.globalAlpha = 1;
            this.renderContext.shadowBlur = 0;
            this.renderContext.globalCompositeOperation = "source-over";
        }

        private drawLine(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            this.renderContext.globalCompositeOperation = "source-over";
            this.renderContext.strokeStyle = data.color ?? "#000";
            this.renderContext.lineWidth = data.lineWidth ?? 1;
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.shadowBlur = data.shadowBlur ?? 0;

            this.renderContext.beginPath();
            this.renderContext.moveTo(command.positionX, command.positionY);
            this.renderContext.lineTo(data.positionEndX ?? command.positionX + 10, data.positionEndY ?? command.positionY + 10);
            this.renderContext.stroke();

            this.renderContext.globalAlpha = 1;
            this.renderContext.shadowBlur = 0;
        }

        private drawText(command: { data: unknown, positionX: number, positionY: number, width: number, height: number }) {
            let data = <any>command.data;

            this.renderContext.fillStyle = data.color ?? "#000";
            this.renderContext.shadowColor = data.color ?? "#000";
            this.renderContext.font = "normal 16pt Arial";
            this.renderContext.textAlign = data.textAlign ?? "left";
            this.renderContext.textBaseline = data.textBaseLine ?? "middle";
            this.renderContext.globalAlpha = data.alpha ?? 1;
            this.renderContext.shadowBlur = data.shadowBlur ?? 0;

            this.renderContext.fillText(data.text ?? "Text", command.positionX, command.positionY);

            this.renderContext.textAlign = "left";
            this.renderContext.globalAlpha = 1;
            this.renderContext.shadowBlur = 0;
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
                } else if (obj.type === "Line") {
                    this.drawLine(obj);
                } else if (obj.type === "Text") {
                    this.drawText(obj);
                }
            }
        }

        public playsound(src: string, volume = 1, loop = false) {
            this.soundLoader.playSound(src, volume, loop);
        }

        update(deltaTime) {
            this.renderContext.clearRect(0, 0, this.canvasWidth, this.canvasHeight);

            this.renderEngine.invokeMethodAsync("UpdateGameObjects", deltaTime / 100.0);

            this.showFPS();
        }

        showFPS() {
            this.renderContext.fillStyle = "White";
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