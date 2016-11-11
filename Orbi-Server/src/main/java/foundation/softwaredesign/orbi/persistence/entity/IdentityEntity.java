package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "identity", schema = "public")
public class IdentityEntity {

    @Id
    @SequenceGenerator(name="identity_id_gen", sequenceName="identity_id_gen")
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "identity_id_gen")
    private Long id;
    @Column
    @NotNull
    private String email;
    @Column
    private String tmpPassword;
    @Column
    private String token;
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    @NotNull
    private Date lastInit;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getTmpPassword() {
        return tmpPassword;
    }

    public void setTmpPassword(String tmpPassword) {
        this.tmpPassword = tmpPassword;
    }

    public Date getLastInit() {
        return lastInit;
    }

    public void setLastInit(Date lastSeen) {
        this.lastInit = lastSeen;
    }

}
