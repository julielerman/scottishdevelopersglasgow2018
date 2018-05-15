using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharedKernel;

namespace Domain {
  public class Team {
    public Team (string teamName, string nickname, string yearFounded, string homeStadium) : this () {
      TeamName = teamName;
      Nickname = nickname;
      YearFounded = yearFounded;
      HomeStadium = homeStadium;
      Id = Guid.NewGuid ();
      //newly created team starts out with empty players
      //team retrieved from database including players will have instantiated Players,
      //whereas from db without including players, Players will be null
      Players = new List<Player> ();
    }
    private Team () {
      // Players=new List<Player>();
    }
    public Guid Id { get; private set; }
    public string TeamName { get; private set; }
    public string Nickname { get; private set; }
    public string YearFounded { get; private set; }
    public string HomeStadium { get; private set; } //encapsulate
    public List<Player> Players { get; private set; }
    public bool AddPlayer (string firstName, string lastname, out string response) {
      if (Players == null) {
        //this will need to be tested with integration test
        response = "You must first retrieve this team's existing list of players";
        return false;
      }
      var fullName = PersonFullName.Create (firstName, lastname).FullName;
      var foundPlayer = Players.Where (p => p.Name.Equals (fullName)).FirstOrDefault ();
      if (foundPlayer == null) {
        Players.Add (new Player (firstName, lastname));
        response = "Player added to team";
        return true;
      } else {
        response = "Duplicate player";
        return false;
      }
    }

    public Manager Manager { get; private set; }
    public UniformColors HomeColors { get; private set; }
    public void ChangeManagement (Manager newManager) {
      {
        if (Manager != null) {
          if (Manager.Name != newManager.Name) {
            Manager.PastTeams.Add(new ManagerTeamHistory(Id, newManager.Id));
          }
        }
        Manager = newManager;
      }
    }
    public void SpecifyHomeUniformColors (Color shirt1, Color shirt2, Color shirt3, Color shorts1, Color shorts2, Color socks) {
      HomeColors = new UniformColors (shirt1, shirt2, shirt3, shorts1, shorts2, socks);
       }
  
  }
}