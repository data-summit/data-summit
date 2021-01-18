import React from 'react';
import ReactPlayer from 'react-player';
import styles from './BackgroundVideo.module.scss';

const BackgroundVideo=({ videoPath, playerWidth, playerHeight }) => {
    
    return(
        <div className={ styles.videoWrapper } id="BackgroundVideo">
            <ReactPlayer
                className ={ styles.reactPlayer } 
                url={ videoPath } 
                muted={ true }
                width={ playerWidth }
                height={ playerHeight }
            />
            <div className={ styles.overlay } id="Overlay">
                <h1 className={ styles.overlayText } id="OverlayText">Text Overlay</h1>
            </div>
        </div>
    );
}

export default BackgroundVideo;