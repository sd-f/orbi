package foundation.softwaredesign.orbi.model;

import javax.persistence.Transient;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */

public class CharacterMessage {

    @Transient
    private Long id;
    private String message;
    private String fromCharacter;
    private Long fromCharacterId;
    private String toCharacter;
    private Long toCharacterId;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getFromCharacter() {
        return fromCharacter;
    }

    public void setFromCharacter(String fromCharacter) {
        this.fromCharacter = fromCharacter;
    }

    public String getToCharacter() {
        return toCharacter;
    }

    public void setToCharacter(String toCharacter) {
        this.toCharacter = toCharacter;
    }

    public Long getFromCharacterId() {
        return fromCharacterId;
    }

    public void setFromCharacterId(Long fromCharacterId) {
        this.fromCharacterId = fromCharacterId;
    }

    public Long getToCharacterId() {
        return toCharacterId;
    }

    public void setToCharacterId(Long toCharacterId) {
        this.toCharacterId = toCharacterId;
    }
}
