package foundation.softwaredesign.orbi.model.auth;

import foundation.softwaredesign.orbi.model.Player;

import javax.validation.constraints.NotNull;
import javax.xml.bind.annotation.XmlRootElement;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class RegisterUserInfo {

    @NotNull
    private String email;
    @NotNull
    private Player player;

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public Player getPlayer() {
        return player;
    }

    public void setPlayer(Player player) {
        this.player = player;
    }
}
