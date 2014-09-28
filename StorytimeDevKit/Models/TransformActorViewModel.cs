using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework;
using StoryTimeCore.Physics;

namespace StoryTimeDevKit.Models
{
    public class TransformActorViewModel : BaseViewModel
    {
        private BaseActor _ba;

        public Vector2 Position
        {
            get
            {
                return _ba.Body.Position;
            }
            set
            {
                if (_ba.Body.Position == value) return;
                _ba.Body.Position = value;
            }
        }

        public float Rotation
        {
            get
            {
                return _ba.Body.Rotation;
            }
            set
            {
                if (_ba.Body.Rotation == value) return;
                _ba.Body.Rotation = value;
            }
        }

        public TransformActorViewModel(BaseActor ba)
        {
            _ba = ba;
            ba.Body.OnPositionChanges += OnPositionChangesHandler;
            ba.Body.OnRotationChanges += OnRotationChangesHandler;
        }

        private void OnPositionChangesHandler(IBody body)
        {
            OnPropertyChanged("Position");
        }

        private void OnRotationChangesHandler(IBody body)
        {
            OnPropertyChanged("Rotation");
        }
    }
}
