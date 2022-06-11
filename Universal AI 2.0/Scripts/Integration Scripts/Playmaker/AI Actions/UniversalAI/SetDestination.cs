#if PLAYMAKER && UniversalAI_Integration_PLAYMAKER
using UnityEngine;
using UniversalAI;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("UniversalAI")]
	[Tooltip("Sets The Temporary Destination For The AI (If The AI Isn't Wandering, AI Will Go To The Destination Whenever It Wanders Again)")]
	public class SetDestination : FsmStateAction
	{

		[RequiredField]
		[CheckForComponent(typeof(UniversalAICommandManager))]
		[Tooltip("The AI GameObject That Has The 'AICommandManager'.")]
		public UniversalAICommandManager AICommandManager;
		[Space]
		
		[Tooltip("The New Destination The AI Will Go.")]
		public FsmGameObject NewDestination = null;
		
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
		}

		public void Execute()
		{
			AICommandManager.SetDestination(NewDestination.Value.transform.position);
		}


	}

}
#endif