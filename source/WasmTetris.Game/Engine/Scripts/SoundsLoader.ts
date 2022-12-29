namespace WasmTetris {
    export class SoundsLoader {
        private soundSources: { [key: string]: HTMLAudioElement } = {};

        loadSounds(soundUrls: string[]) {
            return new Promise(async r => {
                await Promise.all(soundUrls.map(async soundUrl => {

                    let sound = new Audio();

                    sound.setAttribute("preload", "auto");
                    sound.setAttribute("controls", "none");
                    sound.style.display = "none";
                    sound.autoplay = false;
                    sound.src = "sounds/" + soundUrl;

                    this.soundSources[soundUrl] = sound;
                }));
                r(0);
            });
        }

        getSound(soundUrl: string) {
            return this.soundSources[soundUrl];
        }

        playSound(soundUrl: string, volume = 1, loop = false) {
            let sound = this.getSound(soundUrl);
            sound.volume = volume;
            sound.loop = loop;


            sound.currentTime = 0;

            var playedPromise = sound.play();
            if (playedPromise) {
                playedPromise.catch((e) => {
                    setTimeout(() => {
                        this.playSound(soundUrl, volume, loop);
                    }, 100);
                });
            }

        }
    }
}