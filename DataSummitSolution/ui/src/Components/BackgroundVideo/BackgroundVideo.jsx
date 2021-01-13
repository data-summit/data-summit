import React from 'react';
import ReactPlayer from 'react-player';
import styles from './BackgroundVideo.module.scss';
import testMP4 from './Test.mp4';

const BackgroundVideo = () => {
    
    return(
        <div className={ styles.video } id="Video">
            <ReactPlayer
                className={ styles.reactPlayer } 
                url={ testMP4 } 
                muted={true}
                width = '100vw'
                height = '100%'
            />
            <div className={ styles.overlay } id="Overlay">
                <h1 className={ styles.overlayText } id="OverlayText">Text Overlay</h1>
            </div>
        </div>
    );
}

export default BackgroundVideo;