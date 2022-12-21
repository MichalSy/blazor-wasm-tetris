namespace WasmTetris {
    export class FpsCalculator {
        private sampleSize = 60;
        private sampleData = Array<number>();
        private index = 0;
        private lastTick = 0;

        tick(timestamp: number) {
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

            if (this.index === this.sampleSize) this.index = 0;
            return average;
        }
    }
}