#if PLAYMAKER && UniversalAI_Integration_PLAYMAKER
using UnityEngine;
using UniversalAI;

namespace HutongGames.PlayMaker.Actions
{
	
	[ActionCategory("UniversalAI")]
	[Tooltip("This will set the 'Target' of the selected AI manually.")]
	public class SetTarget : FsmStateAction
	{

		[RequiredField]
		[CheckForComponent(typeof(UniversalAICommandManager))]
		[Tooltip("The AI GameObject That Has The 'UniversalAICommandManager'.")]
		public UniversalAICommandManager AICommandManager;
		[Space]

		[Tooltip("The New Target The AI Will Have.")]
		public FsmGameObject NewTarget = null;
		
		
		
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
			NewTarget = null;
		}

		public void Execute()
		{
			AICommandManager.SetTarget(NewTarget.Value);
		}


	}

}
#endif