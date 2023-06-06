using System;
using UnityEngine;

namespace Services.Input
{
    public class PlayerInputService : IPlayerInputService
    {
        private const string InputReaderException = "Input reader is null";

        private IInputReader _inputReader;

        public Vector2 Direction => _inputReader.Direction;
        public float Horizontal => _inputReader.Horizontal;
        public float Vertical => _inputReader.Vertical;
        public void RegisterInputReader(IInputReader inputReader)
        {
            _inputReader = inputReader ?? throw new NullReferenceException(InputReaderException);
            _inputReader.Init();
        }
        
        public void Cleanup()
        {
            _inputReader = null;
        }
    }
}