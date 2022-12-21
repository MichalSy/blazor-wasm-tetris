namespace WasmTetris {
    export class ImageLoader {
        private imageSources: { [key: string]: HTMLImageElement } = {};

        loadImages(imageUrls: string[]) {
            return new Promise(async r => {
                await Promise.all(imageUrls.map(async imageUrl => {
                    let newImage = new Image();
                    let prom = new Promise(r => newImage.onload = r);
                    newImage.src = imageUrl
                    await prom;
                    this.imageSources[imageUrl] = newImage;
                }));
                r(0);
            });
        }

        getImage(imageUrl: string) {
            return this.imageSources[imageUrl];
        }
    }
}