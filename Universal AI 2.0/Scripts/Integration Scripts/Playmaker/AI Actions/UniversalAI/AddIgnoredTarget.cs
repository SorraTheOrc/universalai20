#if PLAYMAKER && UniversalAI_Integration_PLAYMAKER


using UnityEngine;
using UniversalAI;

namespace HutongGames.PlayMaker.Actions
{
	
	[ActionCategory("UniversalAI")]
	[Tooltip("This will add a new Target Object To The 'Ignored Targets'.")]
	public class AddIgnoredTarget : FsmStateAction
	{

		[RequiredField]
		[CheckForComponent(typeof(UniversalAICommandManager))]
		[Tooltip("The AI GameObject That Has The 'AICommandManager'.")]
		public UniversalAICommandManager AICommandManager;
		[Space]
		
		[Tooltip("The New Target The AI Will Ignore.")]
		public FsmGameObject NewIgnoredTarget = null;
		
		public override void OnGUI()
		{
			if(Application.isPlaying)
				return;
			
			if (AICommandManager == null)
			{
				if (Owner.GetComponent<UniversalAICommandManager>() != null)
				{
					AICommandManager = Owner.GetComponent<UniversalAICommandManager>();
				}
			}
		}
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			if(AICommandManager == null)
				return;
			
			Execute();
			Finish();
		}
		
		public override void Reset()
		{
			AICommandManager = null;
			NewIgnoredTarget = null;
		}

		public void Execute()
		{
			AICommandManager.AddIgnoredTarget(NewIgnoredTarget.Value);
		}


	}

}


#endif