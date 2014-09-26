using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using StoryTimeUI.DataBinding;
using StoryTimeUI.DataBinding.Engines;

namespace StoryTimeUITests.DataBinding
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BindingEngineTests
    {
        private class Source : INotifyPropertyChanged
        {
            private int _data;

            public event PropertyChangedEventHandler  PropertyChanged;

            public int Data 
            { 
                get
                {
                    return _data;
                }
                set
                {
                    if (_data == value) return;
                    _data = value;
                    OnPropertyChanged("Data");
                }
            }

            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        private class Destination : INotifyPropertyChanged
        {
            private int _data;

            public event PropertyChangedEventHandler PropertyChanged;

            public int DestinationData
            {
                get
                {
                    return _data;
                }
                set
                {
                    if (_data == value) return;
                    _data = value;
                    OnPropertyChanged("DestinationData");
                }
            }

            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        
        [TestMethod]
        public void BindingWithExpressionsOneWayToDestintionWorks()
        {
            Source source = new Source();
            Destination destination = new Destination();
            BindingEngine<Destination, Source> engine =
                new BindingEngine<Destination, Source>(destination, source)
                .Bind(d => d.DestinationData, s => s.Data);
            source.Data = 5;
            Assert.AreEqual(5, destination.DestinationData);
        }

        [TestMethod]
        public void BindingWithExpressionsOneWayToSourceWorks()
        {
            Source source = new Source();
            Destination destination = new Destination();
            BindingEngine<Destination, Source> engine =
                new BindingEngine<Destination, Source>(destination, source)
                .Bind(d => d.DestinationData, s => s.Data, BindingType.OneWayToSource);
            destination.DestinationData = 5;
            Assert.AreEqual(5, source.Data);
        }

        [TestMethod]
        public void BindingWithPropertyNameOneWayToSourceWorks()
        {
            Source source = new Source();
            Destination destination = new Destination();
            BindingEngine engine = 
                new BindingEngine(destination, source)
                .Bind("DestinationData", "Data", BindingType.OneWayToSource);
            destination.DestinationData = 5;
            Assert.AreEqual(5, source.Data);
        }
    }
}
