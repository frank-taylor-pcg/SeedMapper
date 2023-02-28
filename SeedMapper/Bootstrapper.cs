using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using SeedMapper.ViewModels;

namespace SeedMapper;

public class Bootstrapper : BootstrapperBase
{
	private readonly SimpleContainer _container = new();

	public Bootstrapper()
	{
		Initialize();
	}

	protected override void OnStartup(object sender, StartupEventArgs e)
	{
		DisplayRootViewForAsync<ShellViewModel>();
	}

	protected override void Configure()
	{
		_container
			.Instance(_container)
			.Singleton<IWindowManager, WindowManager>()
			.Singleton<IEventAggregator, EventAggregator>();

		GetType()
			.Assembly
			.GetTypes()
			.Where(type =>
				type.IsClass &&
				type.Name.EndsWith("ViewModel"))
			.ToList()
			.ForEach(viewModelType =>
				_container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
	}

	protected override void BuildUp(object instance)
	{
		_container.BuildUp(instance);
	}

	protected override IEnumerable<object> GetAllInstances(Type service)
	{
		return _container.GetAllInstances(service);
	}
}