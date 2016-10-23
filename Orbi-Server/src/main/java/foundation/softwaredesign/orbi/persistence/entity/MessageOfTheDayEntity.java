package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "motd", schema = "public")
public class MessageOfTheDayEntity {

    @Id
    private Long id;
    @Column
    @NotNull
    private String message;
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    @NotNull
    private Date created;
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    private Date expires;


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

    public Date getCreated() {
        return created;
    }

    public void setCreated(Date created) {
        this.created = created;
    }

    public Date getExpires() {
        return expires;
    }

    public void setExpires(Date expires) {
        this.expires = expires;
    }
}
