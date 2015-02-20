using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ninject;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Models.ParticleEditor;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Utils;

namespace StoryTimeDevKit.Controls.ParticleEditor
{
    /// <summary>
    /// Interaction logic for ParticleEffectControl.xaml
    /// </summary>
    public partial class ParticleEffectTreeView : 
        UserControl, 
        IParticleEffectTreeView
    {
        private IParticleEffectController _particleEffectController;

        public event Action<ParticleTreeViewItem> OnSelectedItemChanged;

        public ICommand SwitchEditMode { get; private set; }
        public ObservableCollection<ParticleProcessorContextViewModel> ParticleProcessors { get; private set; }

        public ParticleEffectTreeView()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        public void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _particleEffectController =
                DependencyInjectorHelper
                    .ParticleEditorKernel
                    .Get<IParticleEffectController>();

            _particleEffectController.ParticleEffectControl = this;
            base.DataContext = _particleEffectController.ParticleEffectViewModel;
            ParticleProcessors = _particleEffectController.ParticleProcessors;

            SwitchEditMode = new RelayCommand(
                (o) =>
                {
                    var item = (o as ParticleTreeViewItem);
                    item.EditMode = !item.EditMode;
                },
                (o) =>
                {
                    return (o as ParticleTreeViewItem) != null;
                }
            );
        }

        private void etb_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var tb = sender as TextBox;

            if (!tb.IsVisible) return;

            //for some reason the textbox has to be rebound when is set to visible again
            var binding = BindingOperations.GetBindingExpressionBase(tb, TextBox.TextProperty);
            binding.UpdateTarget();

            tb.Focus();
        }

        private void etb_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;

            if (string.IsNullOrWhiteSpace(tb.Text)) return;
            //if (_skeletonController.GetBoneViewModelByName(tb.Text) != null) return;

            tb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void ParticleEffect_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (OnSelectedItemChanged == null) return;
            if (!(ParticleEffect.SelectedItem is ParticleTreeViewItem)) return;
            OnSelectedItemChanged(ParticleEffect.SelectedItem as ParticleTreeViewItem);
        }
    }
}
