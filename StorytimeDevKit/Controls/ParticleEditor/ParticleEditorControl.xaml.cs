using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ninject;
using Ninject.Parameters;
using StoryTime;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Controls.Templates;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Utils;

namespace StoryTimeDevKit.Controls.ParticleEditor
{
	/// <summary>
	/// Interaction logic for ParticleEditorControl.xaml
	/// </summary>
	public partial class ParticleEditorControl : BaseSceneUserControl, IParticleEditorControl
	{
		private MyGame _game;
		private IParticleEditorController _particleController;

		public ParticleEditorControl()
		{
			InitializeComponent();
			AssignPanelEventHandling(ParticleEditor);
			Loaded += LoadedHandler;
		}

		private void LoadedHandler(object sender, RoutedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(this))
				return;

			_game = new MyGame(ParticleEditor.Handle);
			
			var controlArg =
				new ConstructorArgument(
					ApplicationProperties.IParticleEditorControllerGameWorldArgName,
					_game.GameWorld);

			_particleController =
				DependencyInjectorHelper
					.ParticleEditorKernel
					.Get<IParticleEditorController>(controlArg);

			_particleController.ParticleEditorControl = this;

			/*_transformModeModel =
				DependencyInjectorHelper
					.ParticleEditorKernel
					.Get<TransformModeViewModel>();

			TranslateButton.DataContext = _transformModeModel;
			FreeMovementButton.DataContext = _transformModeModel;
			RotateButton.DataContext = _transformModeModel;
			ScaleButton.DataContext = _transformModeModel;*/
		}

        private void AddParticleEmitter_Click(object sender, RoutedEventArgs e)
        {
            _particleController.AddParticleEmitter();
        }
	}
}
