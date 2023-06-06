using System;
using UnityEngine;

namespace Services.Input
{
    public class PlayerInputService : IPlayerInputService
    {
        private const string InputReaderException = "Input reader is null";

        private IInputReader _inputReader;

        public void RegisterInputReader(IInputReader inputReader)
        {
            _inputReader = inputReader ?? throw new NullReferenceException(InputReaderException);
            _inputReader.Init();
        }

        public Vector2 Direction => _inputReader.Direction;

        public void Cleanup()
        {
            _inputReader = null;
        }
    }
}