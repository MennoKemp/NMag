using Auxilia.Delegation.Commands;
using System;

namespace NMag.Commands
{
	public interface ICommandFactory<T> where T : CommandBase
	{
		T CreateCommand(Type commandType);
	}
}