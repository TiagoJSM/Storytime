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
        }

        public TransformActorViewModel(BaseActor ba)
        {
            _ba = ba;
            ba.Body.OnPositionChanges += OnPositionChangesHandler;
        }

        private void OnPositionChangesHandler(IBody body)
        {
            OnPropertyChanged("Position");
        }
    }
}
