import React from 'react';
import './App.css';
import Video from './Components/BackgroundVideo/BackgroundVideo';

function App() {
  return (
    <div className="App">
      <header className="App-header">
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
          <Video></Video>
      </section>
    </div>
  );
}

export default App;
