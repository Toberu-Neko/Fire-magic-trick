using MoreMountains.Feedbacks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySpawn_GlassBox : MonoBehaviour
{
    public enum Fightstate
    {
        Fight_Null,
        A,
        B,
        C,
        End,
    }
    [MMFReadOnly] public Fightstate state;
    private EnemySpawn_Pipe[] pipes;
    public List<EnemySpawn_Pipe> workPipes;

    [Header("Enemy")]
    [SerializeField] private GameObject Enemy_A;
    [SerializeField] private GameObject Enemy_B;
    [SerializeField] private GameObject Enemy_C;

    [Header("Event")]
    public TriggerArea_DialogueTrigger[] Dialogues;
    private int dialogueNumber;
    [SerializeField] private int EndTime;

    private void Awake()
    {
        pipes = GetComponentsInChildren<EnemySpawn_Pipe>();
    }
    
    public void StartFight()
    {
        if(state == Fightstate.Fight_Null)
        {
            state = state + 1;
            Fight(state);
        }
    }
    
    public void Fight(Fightstate state)
    {
        triggerDialogue();

        switch (state)
        {
            case Fightstate.A:
                playPipe(pipes[1], Enemy_A, 2, 0.5f);
                playPipe(pipes[3], Enemy_A, 2, 0.75f);
                playPipe(pipes[5], Enemy_B, 2, 1f);
                playPipe(pipes[7], Enemy_C, 1, 2f);
                break;

            case Fightstate.B:
                playPipe(pipes[0], Enemy_A, 1, 0.5f);
                playPipe(pipes[1], Enemy_A, 1, 0.75f);
                playPipe(pipes[2], Enemy_A, 1, 1f);
                playPipe(pipes[3], Enemy_A, 1, 0.5f);
                playPipe(pipes[4], Enemy_B, 2, 2f); 
                playPipe(pipes[5], Enemy_B, 2, 2f);
                playPipe(pipes[6], Enemy_C, 1, 1f);
                playPipe(pipes[7], Enemy_C, 1, 2f);
                break;

            case Fightstate.C:
                playPipe_keep();
                break;

            case Fightstate.End:

                break;
        }
    }
    private void triggerDialogue()
    {
        Dialogues[dialogueNumber].EventTrigger();
        dialogueNumber++;
    }
    private void playPipe(EnemySpawn_Pipe pipe,GameObject Enemy,int number,float CD)
    {
        workPipes.Add(pipe);
        pipe.onPipeFightover += onFightOverCheck;
        pipe.resetPipe(Enemy,number, CD);
        pipe.ToSpawn();
    }
    private void playPipe_keep()
    {
        for(int i = 0; i < pipes.Length; i++)
        {
            pipes[i].ChangeMode(EnemySpawn_Pipe.spawnMode.Keep);
            pipes[i].resetPipe(Enemy_A, 10, 4);
            pipes[i].ToSpawn();
            workPipes.Add(pipes[i]);
        }
        ToEnd();
    }
    private async void ToEnd()
    {
        await Task.Delay(EndTime*1000);
        state++;
        Fight(state);

        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].StopSpawn();
        }
    }
    private void onFightOverCheck()
    {
        //if fight not over,return
        foreach (EnemySpawn_Pipe pipe in workPipes)
        {
            if(pipe.isFightOver == false)
            {
                return; 
            }
        }

        //when over,clear event and list.
        
        workPipes.Clear();

        //state to next.
        state++;
        Fight(state);
        Debug.Log("To Next");
    }
}
