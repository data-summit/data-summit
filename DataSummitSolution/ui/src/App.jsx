import React from 'react'
import BackgroundVideo from './Components/BackgroundVideo/BackgroundVideo';
import styles from './App.module.scss';
import mp4 from './Test.mp4';

function App() {
  return (
    <div className="App">
      <header className= { styles.AppHeader }>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>  
      </header>
      <section>
          <BackgroundVideo 
            videoPath={ mp4, '100vw', '100%' }
          />
      </section>
    </div>
  );
}

export default App;
