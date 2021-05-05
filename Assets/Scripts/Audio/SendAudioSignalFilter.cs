using UnityEngine;

namespace Audio
{
    public class SendAudioSignalFilter : MonoBehaviour
    {
        public float sineFrequency;
        [Range(0.0f, 1.0f)]
        public float sineGain;
        public int sendBatches;

        private AudioGenerator _audioGenerator;
    
        private int _batches;
        private object _lockObject = new object();
    
        void Awake()
        {
            _batches = 0;
            _audioGenerator = new AudioGenerator(AudioSettings.outputSampleRate);
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            bool send;
            lock (_lockObject)
            {
                send = _batches > 0;
            }
        
            if (send)
            {
//                _audioGenerator.WriteSineWave(data, sineFrequency, sineGain, 0, channels);
                _audioGenerator.WriteSineWaveAllChannels(data, sineFrequency, sineGain, channels);
            
                lock (_lockObject)
                {
                    _batches--;
                }
            }
        }

        public void Send(int batches)
        {
            lock (_lockObject)
            {
                _batches = batches;
            }
        }

        [NaughtyAttributes.Button]
        public void Send()
        {
            Send(sendBatches);
        }

        [NaughtyAttributes.Button]
        public void Stop()
        {
            lock (_lockObject)
            {
                _batches = 0;
            }
        }

        public bool IsSending()
        {
            bool sending = false;
            lock (_lockObject)
            {
                sending = _batches > 0;
            }

            return sending;
        }
    }
}
